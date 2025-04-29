using Assets.Scripts.Controller;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public class PlayerData
    {
        public string ship;
    }

    public class Player : MonoBehaviour
    {
        private PlayerData data;
        string path;

        public PlayerController controller;

        public static ShipData shipData;
        public static Ship ship;


        private void Awake()
        {
            data = new PlayerData();
            path = Application.persistentDataPath + "/player.dat";
            
            shipData = Repo.Load("Player");
        }


        private void Start()
        {

            if (!Load())
            {
                data.ship = "Corsair";
                Spawn();
            }
            else
                Spawn();
        }

        private void Spawn()
        {
            Spawns.Get().shipSpawnFunctions[data.ship]("Player");
            Spawns.Get().lastSpawnedShip.IsPlayer = true;
            Spawns.Get().lastSpawnedShip.controller = controller;
            Spawns.Get().lastSpawnedShip.layer = "Player";
            CameraController.Instance.SetPlayerShip(Spawns.Get().lastSpawnedShip);
            Player.ship = Spawns.Get().lastSpawnedShip;
        }

        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);


            stream.Close();
        }

        public bool Load()
        {
            if(File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                data = formatter.Deserialize(stream) as PlayerData;

                stream.Close();
           

                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnApplicationQuit()
        {

            data.ship = ship.shipClass;

            Save();
        }
    }
}
