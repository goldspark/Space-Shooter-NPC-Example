using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering;
using JetBrains.Annotations;
using UnityEngine.LowLevelPhysics;

namespace Assets.Scripts.Utils
{

    public class ShipData
    {
        public string shipClass;
        public float currentHp;
        public float maxHp;
        public float maxSpeed;
        public float evade;
        public float turnSpeed;
        public float maxSpeedIncreaseRate;
        public float maxSpeedDecreaseRate;
        public Dictionary<string, GameObject> turrets = new Dictionary<string, GameObject>(); // key - hardpoint name
        public Dictionary<string, string> assetLocations = new Dictionary<string, string>(); // key - hardpoint name
        public List<Item> items = new List<Item>();
    }

    public static class Repo
    {

        public static Dictionary<string, ShipData> ships = new Dictionary<string, ShipData>(); //key - ship name

        private static string defaultGun = "Weapons/SimpleGun";
        private static string defaultTur = "Weapons/SimpleTurret";


        public static ShipData Load(string Name)
        {
            string path = Application.persistentDataPath + "/ents.dat";

            if (!File.Exists(path))
            {
                return null;
            }


            INIParser parser = new INIParser();

            parser.Open(path);

            if (!parser.IsSectionExists(Name))
                return null;


            Item.ID = 0;

            ShipData data = new ShipData();

            data.shipClass = parser.ReadValue(Name, "ShipClass", "");
            data.currentHp = (float)parser.ReadValue(Name, "HP", 0);
            data.maxHp = (float)parser.ReadValue(Name, "MaxHP", 0);
            data.maxSpeed = (float)parser.ReadValue(Name, "MaxSpeed", 0);
            data.evade = (float)parser.ReadValue(Name, "Evade", 0);
            data.turnSpeed = (float)parser.ReadValue(Name, "TurnSpeed", 0);
            data.maxSpeedIncreaseRate = (float)parser.ReadValue(Name, "MaxSpeedIncreaseRate", 0);
            data.maxSpeedDecreaseRate = (float)parser.ReadValue(Name, "MaxSpeedDecreaseRate", 0);



            for (int i = 0; i < 10; i++)
            {
                string assetLocation = parser.ReadValue(Name, $"Hardpoint{i}", "");

                if (parser.ReadValue(Name, $"Hardpoint{i}", "").Length == 0)
                {
                    continue;
                }

                string hardpointName = $"Hardpoint{i}";

                data.assetLocations[hardpointName] = assetLocation;               
            }
            

            foreach (KeyValuePair<string, string> pair in data.assetLocations)
            {
                string assetLocation = pair.Value;
                string hardpointName = pair.Key;

                data.turrets[hardpointName] = Resources.Load<GameObject>(assetLocation);
            }

            string itemDescription = ""; // mounted, isTurret, assetPath, name, hardpointName
            int count = parser.ReadValue(Name, "Count", -1);
            for (int i = 0; i < count; i++)
            {
                itemDescription = parser.ReadValue(Name, "Item" + i, "");
                if (itemDescription.Length < 1)
                    continue;

                string[] tokens = itemDescription.Split(',');

                bool isMounted = bool.Parse(tokens[0]);
                bool isTurret = bool.Parse(tokens[1]);
                string prefabLoc = tokens[2];
                string itemName = tokens[3];
                string hardPointName = tokens[4];

                
                Item item = new Item();
                item.isMounted = isMounted;
                item.isTurret = isTurret;
                item.prefabLocation = prefabLoc;
                item.Name = itemName;
                item.hardpointName = hardPointName;
                data.items.Add(item);
            }

            parser.Close();

            ships[Name] = data;

            return data;
        }


        public static void LoadDefaultEquipment(Ship ship)
        {
            string path = "DATA/Ships.ini";
            INIParser parser = new INIParser();

            if (!File.Exists(path))
                Debug.Log(path + " does not exists");

            parser.Open(path);

            for (int i = 0; i < ship.hardPoints.Count; i++)
            {
                string asset = parser.ReadValue(ship.shipClass, $"Hardpoint{i}", "");
                if (asset.Length == 0)
                    continue;

                Transform hardpointParent = ship.transform.Find($"Hardpoint{i}");
                var o = GameObject.Instantiate(Resources.Load<GameObject>(asset), hardpointParent);
                Turret turret = o.GetComponent<Turret>();
                turret.owner = ship;
                turret.isTurret = hardpointParent.gameObject.GetComponent<Hardpoint>().isTurret;
            }

            parser.Close();
        }


        public static bool LoadShip(Ship ship)
        {

            if (ship == null || !ships.ContainsKey(ship.Name))
            {
                return false;
            }

            ShipData data = ships[ship.Name];

            ship.shipClass = data.shipClass;
            ship.currentHp = data.currentHp;
            ship.maxHp = data.maxHp;
            ship.MaxSpeed = data.maxSpeed;
            ship.evade = data.evade;
            ship.TurnSpeed = data.turnSpeed;
            ship.MaxSpeedIncreaseRate = data.maxSpeedIncreaseRate;
            ship.MaxSpeedDecreaseRate = data.maxSpeedDecreaseRate;


            ship.inventory.items.Clear();

            foreach (KeyValuePair<string, GameObject> t in data.turrets)
            {

                Transform hardPoint = GameObject.Find(t.Key).transform;

                var o = GameObject.Instantiate(t.Value, hardPoint);
                Turret turret = o.GetComponent<Turret>();
                turret.owner = ship;
                turret.isTurret = hardPoint.gameObject.GetComponent<Hardpoint>().isTurret;
            }


            Debug.Log("Unmounted weapons to add " + data.items.Count);

            for (int i = 0; i < data.items.Count; i++)
            {
                ship.inventory.AddItem(data.items[i]);
            }


            return true;

        }


    }
}
