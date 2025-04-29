using Assets.Scripts.BehaviorTree.AI;
using Assets.Scripts.BehaviorTree.AIEntities;
using Assets.Scripts.Controller;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using BehaviorTree;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Assets.Scripts.Entities
{

    public enum Team
    {
        TEAM1,
        TEAM2,
        TEAM3
    }

    public enum Type
    {
        FIGHTER,
        CAPITAL
    }

    public class Ship : MonoBehaviour, IComparable<Ship>
    {
        [Header("Ship Initialization")]
        public string Name;
        public string shipClass;
        public Team team;
        public Type type = Type.FIGHTER;   
        public SpaceShipController controller;

        public Inventory inventory;

        public string layer = "Ship";

        [HideInInspector]
        public AIEntity Entity;

        public List<Transform> hardPoints;

        [HideInInspector]
        public Turret selectedTurret;
        [Tooltip("This is for guns only.")]
        public List<Turret> Turrets;

        [HideInInspector]
        public List<Turret> mountedTurrets = new List<Turret>();
        /// <summary>
        /// When ship explodes the effect to appear
        /// </summary>
        public GameObject explosionParticle;


        public float maxHp = 1000;
        [HideInInspector]
        public float currentHp;

        [HideInInspector]   
        public bool disableAI = false;

        private bool _explosionSpawned = false;
        
        public float MaxSpeed = 100f;
        public float MinSpeed = -30f;
        /// <summary>
        /// Shows current speed
        /// </summary>
        [HideInInspector]
        public float CurrentSpeed = 0f;

        public float MaxSpeedIncreaseRate = 50f;
        public float MaxSpeedDecreaseRate = 100f;

        public float evade = 10f; 

        [HideInInspector]
        public float speedIncreaseRate = 0f;
        /// <summary>
        /// How fast it does pitch up/down and turns left/right
        /// </summary>
        [SerializeField]
        public float TurnSpeed = 50f;

        public Renderer EngineRenderer;

        public bool IsPlayer = false;

        public RadarBlip blip;

        [HideInInspector]
        public List<int> detectedShips = new List<int>();
        [HideInInspector]
        public List<int> detectedBy = new List<int>();
        [HideInInspector]
        public Ship curretTarget;

        private Mission _missionRef;



        private void Awake()
        {
            inventory = new Inventory(this);

            hardPoints.Clear();
            foreach (Hardpoint p in GetComponentsInChildren<Hardpoint>())
            {
                hardPoints.Add(p.transform);
            }         


            if (!Repo.LoadShip(this))
                Repo.LoadDefaultEquipment(this);
        }

        private void Start()
        {

            gameObject.layer = LayerMask.NameToLayer(layer);


            if (IsPlayer)
            {
                Player.ship = this;
            }

            if (IsPlayer)
                Entity = new AIEntity(null, this);
            else if (type == Type.FIGHTER)
                Entity = new AIEntity(new Fighter(), this);
            else
                Entity = new AIEntity(new Gunboat(), this);

            if (CameraController.Instance != null)
                _missionRef = GameObject.Find("Missions").GetComponent<Mission>();


            currentHp = maxHp;

            UpdateMountedTurrets();
            UpdateAvailableGuns();
            inventory.AddEquippedWeapons(this);

        


            //Set up the controller
            if (controller != null)
            {
                controller = Instantiate(controller);
                controller.transform.SetParent(transform);
                controller.Entity = this;
                controller.rb = GetComponent<Rigidbody>();
            }

        }

        public void UpdateMountedTurrets()
        {

            //Load mounted turrets
            mountedTurrets.Clear();
            foreach (Transform t in hardPoints)
            {
                if (t.childCount > 0)
                {
                    t.gameObject.GetComponent<Hardpoint>().assetPath = t.GetChild(0).gameObject.GetComponent<Turret>().assetPath;
                    mountedTurrets.Add(t.GetChild(0).gameObject.GetComponent<Turret>());
                    mountedTurrets.Last().owner = this;
                }
            }       
     
        }

        public void UpdateAvailableGuns()
        {
            Turrets.Clear();
            foreach (Turret t in mountedTurrets)
            {
                if (!t.isTurret)
                {
                    Turrets.Add(t);
                }
            }

            if (Turrets.Count > 0)
                selectedTurret = Turrets[0];
        }




        private void Update()
        {

           

            if (currentHp < 0)
            {
                if (IsPlayer && detectedBy.Count > 0)
                {
                    CameraController.Instance.LookAtShip(AIEntityManager.Get().GetEntity(detectedBy[0]).Owner as Ship);
                }
                else if(!IsPlayer)
                {
                    _missionRef.enemyShipList.Remove(this);
                    if(blip != null)
                        Destroy(blip.gameObject);
                }

                foreach (int s in detectedBy)
                    (AIEntityManager.Get().GetEntity(s).Owner as Ship).detectedShips.Remove(Entity.GetID());


                SpawnExplosion();
                StartCoroutine(ScaleOverTime(transform, explosionParticle.GetComponent<ParticleSystem>().main.duration - 5.0f, 0.1f));
                disableAI = true;
           
            }


            if (EngineRenderer != null)
                EngineEffect();

            if (!IsPlayer && !disableAI)
                Entity.Update(Time.deltaTime);


        }

        private void EngineEffect()
        {
            EngineRenderer.material.color = new Color(0, 0, CurrentSpeed/MaxSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            Ship otherShip = other.gameObject.GetComponent<Ship>();
            if (otherShip == null || otherShip.team == team || otherShip.Entity.GetID() == Entity.GetID())
            {
                if(IsPlayer && otherShip != null && otherShip.team == team && otherShip.Entity.GetID() != Entity.GetID()
                    && otherShip.blip == null)
                {
                    otherShip.blip = Instantiate(HUD.Instance.friendlyBlip);
                    otherShip.blip.transform.SetParent(HUD.Instance.transform);
                    otherShip.blip.ship = otherShip;
                }
                return;
            }

            if (detectedShips.Contains(otherShip.Entity.GetID()))
                return;

            detectedShips.Add(otherShip.Entity.GetID());
            otherShip.detectedBy.Add(Entity.GetID());

            //Add blip to hud
            if(IsPlayer && otherShip.blip == null)
            {
                otherShip.blip = Instantiate(HUD.Instance.enemyBlip);
                otherShip.blip.transform.SetParent(HUD.Instance.transform);
                otherShip.blip.ship = otherShip;
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.isTrigger)
                return;

            Ship otherShip = other.gameObject.GetComponent<Ship>();
            if (otherShip == null || otherShip.Entity.GetID() == Entity.GetID())
                return;

            if (detectedShips.Contains(otherShip.Entity.GetID()))
            {
                detectedShips.Remove(otherShip.Entity.GetID());
                otherShip.detectedBy.Remove(Entity.GetID());
            }
        }

         
        public void ShootAt(Vector3 target)
        {
            foreach (Turret t in Turrets)
            {
                t.AimAt(target);
                t.Shoot();
            }
        }

        public void AimAt(Vector3 target)
        {
            foreach (Turret t in Turrets)
                t.AimAt(target);       
        }

        private void SpawnExplosion()
        {
            if (!_explosionSpawned)
            {
                var particle = Instantiate(explosionParticle, transform, false);
                particle.GetComponent<Explosion>().owner = this;
                _explosionSpawned = true;
            }
        }

        private IEnumerator ScaleOverTime(Transform transform, float duration, float scale)
        {
            var startScale = transform.localScale;
            var endScale = Vector3.one * scale;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                var t = elapsed / duration;
                transform.localScale = Vector3.Lerp(startScale, endScale, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = endScale;
        }

        public int CompareTo(Ship other)
        {
            return other.Entity.GetID().CompareTo(Entity.GetID());     
        }

        private void OnApplicationQuit()
        {
            //For now only save player
            if(IsPlayer)
                Save();
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/ents.dat";     
            INIParser parser = new INIParser();

            parser.Open(path);

            //Update by first removing last save
            parser.SectionDelete(Name);

            parser.WriteValue(Name, "ShipClass", shipClass);
            parser.WriteValue(Name, "HP", currentHp);
            parser.WriteValue(Name, "MaxHP", maxHp);
            parser.WriteValue(Name, "MaxSpeed", MaxSpeed);
            parser.WriteValue(Name, "Evade", evade);
            parser.WriteValue(Name, "TurnSpeed", TurnSpeed);
            parser.WriteValue(Name, "MaxSpeedIncreaseRate", MaxSpeedIncreaseRate);
            parser.WriteValue(Name, "MaxSpeedDecreaseRate", MaxSpeedDecreaseRate);
            for (int i = 0; i < hardPoints.Count; i++)
            {
                string assetPath = hardPoints[i].gameObject.GetComponent<Hardpoint>().assetPath;
                if (hardPoints[i].childCount == 0)
                {
                    assetPath = "";
                }

                parser.WriteValue(Name, hardPoints[i].name, assetPath);
            }

            string itemDescription = ""; // mounted, isTurret, assetPath, name, hardpointName
            int count = 0;


            List<Item> unMountedItems = new List<Item>();
            for(int i = 0; i < inventory.items.Count; i++)
            {
                Item item = inventory.items[i];
                if(!item.isMounted)
                    unMountedItems.Add(item);
            }


            for (int i = 0; i < unMountedItems.Count; i++)
            {
                Item item = unMountedItems[i];
                if (item.isMounted)
                    continue;
                itemDescription = item.isMounted + "," + item.isTurret + "," + item.prefabLocation + "," + item.Name + "," + item.hardpointName;
                parser.WriteValue(Name, "Item" + i, itemDescription);
                count++;
            }
            parser.WriteValue(Name, "Count", count);

            
            parser.Close();


        }

        //public bool Load()
        //{
        //    string path = Application.persistentDataPath + "/ents.dat";

        //    if (!File.Exists(path))
        //    {
        //        return false;
        //    }
      

        //    INIParser parser = new INIParser();

        //    parser.Open(path);

        //    if (!parser.IsSectionExists(Name))
        //        return false;

        //    //Remove prefabs that come preloaded with ship because we want saved stuff
        //    foreach (Transform t in hardPoints)
        //    {
        //        Destroy(t.GetChild(0).gameObject);
        //    }

        //    shipClass = parser.ReadValue(Name, "ShipClass", shipClass);
        //    currentHp = (float)parser.ReadValue(Name, "HP", currentHp);
        //    maxHp = (float)parser.ReadValue(Name, "MaxHP", maxHp);
        //    MaxSpeed = (float)parser.ReadValue(Name, "MaxSpeed", MaxSpeed);
        //    evade = (float)parser.ReadValue(Name, "Evade", evade);
        //    TurnSpeed = (float)parser.ReadValue(Name, "TurnSpeed", TurnSpeed);
        //    MaxSpeedIncreaseRate = (float)parser.ReadValue(Name, "MaxSpeedIncreaseRate", MaxSpeedIncreaseRate);
        //    MaxSpeedDecreaseRate = (float)parser.ReadValue(Name, "MaxSpeedDecreaseRate", MaxSpeedDecreaseRate);

        //    int i = 0;
        //    while(i < hardPoints.Count)
        //    {
        //        string assetLocation = parser.ReadValue(Name, hardPoints[i].name, "");

        //        if(parser.ReadValue(Name, hardPoints[i].name, "").Length == 0)
        //        {
        //            Debug.Log($"Could not find weapon for {hardPoints[i].name} to load");
        //            i++;
        //            continue;
        //        }

        //        Transform hardPoint = GameObject.Find(hardPoints[i].name).transform;

        //        Addressables.LoadAssetAsync<GameObject>(assetLocation).Completed += (AsyncOperationHandle<GameObject> obj) => {

                    

        //            if (obj.Status == AsyncOperationStatus.Succeeded)
        //            {
        //                var o = Instantiate(obj.Result, hardPoint);
        //                Turret turret = o.GetComponent<Turret>();
        //                turret.owner = this;
        //                turret.isTurret = hardPoint.gameObject.GetComponent<Hardpoint>().isTurret;

        //                UpdateMountedTurrets();
        //                UpdateAvailableGuns();
        //                inventory.AddEquippedWeapons(this);
        //                i++;
        //            }
        //            else
        //            {
        //                Debug.Log("Could not load turret");
        //                i++;
        //            }
                    
        //        };
        //    }


        //    string itemDescription = ""; // mounted, isTurret, assetPath, name, hardpointName
        //    int count = parser.ReadValue(Name, "Count", -1);
        //    for (i = 0; i < count; i++)
        //    {
        //        itemDescription = parser.ReadValue(Name, "Item" + i, "");
        //        if (itemDescription.Length < 1)
        //            continue;

        //        string[] tokens = itemDescription.Split(',');

        //        bool isMounted = bool.Parse(tokens[0]);
        //        bool isTurret = bool.Parse(tokens[1]);
        //        string prefabLoc = tokens[2];
        //        string itemName = tokens[3];
        //        string hardPointName = tokens[4];

        //        Item item = new Item(inventory.itemID++);
        //        item.isMounted = isMounted;
        //        item.isTurret = isTurret;
        //        item.prefabLocation = prefabLoc;
        //        item.Name = itemName;
        //        item.hardpointName = hardPointName;
        //        inventory.AddItem(item);
        //        Debug.Log("Added " + item.Name);
        //    }

        //    parser.Close();

        //    return true;
        //}

        

    }
}
