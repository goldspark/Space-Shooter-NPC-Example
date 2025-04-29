using Assets.Scripts.Entities;
using Assets.Scripts.Game;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Mission : MonoBehaviour
{
    private static int GLOBAL_ID = 0;
    private int _id;
    private int _level = 0;
    private int _innerLevel = 0;

    public const int MAX_LEVEL = 10;
    public Dictionary<string, string> weaponRewards = new Dictionary<string, string>();
    public List<Ship> enemyShipList = new List<Ship>();
    public HUD hud;
    private INIParser _parser = new INIParser();

    private Ship playerShip;

    public int timesSpawn = 1;

    private bool _start;
    private bool _missionStarted = false;

    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _id = GLOBAL_ID++;
    }

    private void Start()
    {
    }


    private void Update()
    {
        if (!_missionStarted)
            return;

        if(!_start && enemyShipList.Count == 0)
        {
            PrizeDialog.Instance.Show();
            _innerLevel++;
            _level++;
            BeginMission();
        }
    }


    private void BeginMission()
    {

        playerShip.currentHp = playerShip.maxHp;
        hud.level.text = "" + _innerLevel;
        enemyShipList.Clear();
        _parser.Open("MISSIONS\\Missions.txt");

        string key = "Mission" + _level;

        //Check for rewards
        string previousMission = "Mission" + (_level - 1);
        if (_level > 0 && _parser.IsKeyExists(previousMission, "Rewards"))
        {
            string[] rewardInfo = _parser.ReadValue(previousMission, "Rewards", "").Split(',');
            string[] rewardCategories = new string[] {"Upgrade", "Items"};

            //Get the category
            if (rewardInfo.Length > 0)
            {
                for(int i = 0; i < rewardInfo.Length; i++)
                {
                    string category = null;
                    foreach(string s in rewardCategories)
                    {
                        if(rewardInfo[i].Equals(s))
                        {
                            category = s; 
                            break;
                        }
                    }

                    if (category == null)
                        continue;

                    i++;


                    switch (category)
                    {
                        case "Upgrade":
                            {
                                if (rewardInfo[i].Equals("Speed"))
                                {
                                    Player.ship.MaxSpeed += 20.0f;
                                }
                            }
                            break;
                        case "Items":
                            {
                                Turret turr = Resources.Load<Turret>(rewardInfo[i]);   
                                
                                Item item = new Item();
                                item.Name = turr.Name;
                                item.hardpointName = "";
                                item.isMounted = false;
                                item.isTurret = turr.isTurret;
                                item.prefabLocation = rewardInfo[i];

                                Player.ship.inventory.AddItem(item);
                               
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            
        }

        if (!_parser.IsSectionExists(key))
        {
            timesSpawn++;
            _level = 0;
        }

        for(int i = 0; i < timesSpawn; i++)
            SpawnShips(key, "ShipsToSpawnT2", Team.TEAM2);

        //Load fleet rewards
       
        string[] fleetInfo = _parser.ReadValue(key, "Fleet", "").Split(',');

        bool isFleetExist = true;
        if (!_parser.IsKeyExists(key, "Fleet"))
        {
            HUD.Instance.prizeDialog.btnShip.gameObject.GetComponent<CanvasGroup>().interactable = false;
            isFleetExist = false;
        }
        else
            HUD.Instance.prizeDialog.btnShip.gameObject.GetComponent<CanvasGroup>().interactable = true;

        _parser.Close();

        if (!isFleetExist)
            return;

        HUD.Instance.prizeDialog.shipsToSpawn.Clear();
        for (int i = 0; i < fleetInfo.Length;i++)
        {
            int amount = int.Parse(fleetInfo[i]);
            string shipName = fleetInfo[++i];

            for (int j = 0; j < amount; j++)
            {
                HUD.Instance.prizeDialog.shipsToSpawn.Add(shipName);
            }
        }

    }

    //Spawn format 
    private void SpawnShips(string section, string key, Team team)
    {

        string shipsToSpawn = _parser.ReadValue(section, key, "");    
        string[] shipsWithCount = shipsToSpawn.Split(',');
        for (int i = 0; i < shipsWithCount.Length; i++)
        {
            int num = int.Parse(shipsWithCount[i]);
            string shipName = shipsWithCount[++i];

            int j = 0;
            while (j < num)
            {
                Spawns.Get().shipSpawnFunctions[shipName](shipsWithCount[++i]);
                Spawns.Get().lastSpawnedShip.team = team;
                Spawns.Get().lastSpawnedShip.transform.position = playerShip.transform.position - playerShip.transform.forward * 3000f; 
                enemyShipList.Add(Spawns.Get().lastSpawnedShip);
                _start = false;
                j++;
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        GameEvents.Instance.OnUse -= StartMission;


        Ship otherShip = other.gameObject.GetComponent<Ship>();
        if (otherShip != null && otherShip.IsPlayer)
        {
            HUD.Instance.yesNo.StopText();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || _missionStarted)
            return;

        GameEvents.Instance.OnUse += StartMission;


        Ship otherShip = other.gameObject.GetComponent<Ship>();
        if (otherShip != null && otherShip.IsPlayer)
        {
            HUD.Instance.yesNo.SetText("Press E to start a mission", true, _id);
        }
    }

    public void StartMission(int id)
    {

        if (_id != id)
            return;
        if (!_missionStarted)
        {

            HUD.Instance.yesNo.StopText();

            _sphereCollider.enabled = false;
            playerShip = Player.ship;
            _start = true;
            _missionStarted = true;
            BeginMission();
        }
    }


}
