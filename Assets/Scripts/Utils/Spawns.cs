using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public sealed class Spawns : MonoBehaviour
    {
        private static Spawns _instance;

        public Ship dragonfly;
        public Ship fork;
        public Ship victory;
        public Ship legate;
        public Ship libBoat;
        public Ship gunboat;
        public Ship battleship;
        public Transform aiSpawnTransform;

        public Turret basicTurret;
        public Turret basicGun;
        public Turret advTurret;
        public Turret advGun;

        public Ship lastSpawnedShip = null;

        public Dictionary<string, Action<string>> shipSpawnFunctions = new Dictionary<string, Action<string>>()
        {
            {"Dragonfly", SpawnDragonfly},
            {"Fork", SpawnFork},
            {"Gunboat",  SpawnGunboat},
            {"Liberty", SpawnLiberty },
            {"Dreadnaught", SpawnBattleship },
            {"Corsair", SpawnCorsair },
            {"Victory", SpawnVictory }
        };

        public Dictionary<string, Turret> weaponSpawnFunctions;


        private void Awake()
        {
            _instance = this;

            weaponSpawnFunctions = new Dictionary<string, Turret>()
            {
                {"BasicTurret", _instance.basicTurret },
                {"AdvancedTurret", _instance.advTurret},
                {"BasicGun", _instance.basicGun },
                {"AdvancedGun", _instance.advGun},
            };

        }

        public static Spawns Get()
        {
            return _instance;
        }

        private static void SpawnDragonfly(string name)
        {
            Get().dragonfly.Name = name;
            Get().dragonfly.transform.position = Vector3.zero;
            Get().dragonfly.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().dragonfly);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);

        }

        private static void SpawnFork(string name)
        {
            Get().fork.transform.position = Vector3.zero;
            Get().fork.Name = name;
            Get().fork.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().fork);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);

        }

        private static void SpawnCorsair(string name)
        {
            Get().legate.transform.position = Vector3.zero;
            Get().legate.Name = name;
            Get().legate.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().legate);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);
        }

        private static void SpawnVictory(string name)
        {
            Get().victory.transform.position = Vector3.zero;
            Get().victory.Name = name;
            Get().victory.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().victory);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);
        }

        private static void SpawnBattleship(string name)
        {
            Get().battleship.transform.position = Vector3.zero;
            Get().battleship.Name = name;
            Get().battleship.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().battleship);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);
        }

        private static void SpawnGunboat(string name)
        {
            Get().gunboat.transform.position = Vector3.zero;
            Get().gunboat.Name = name;
            Get().gunboat.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().gunboat);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);
        }

        private static void SpawnLiberty(string name)
        {
            Get().libBoat.transform.position = Vector3.zero;
            Get().libBoat.Name = name;
            Get().libBoat.team = Team.TEAM1;

            Get().lastSpawnedShip = GameObject.Instantiate(Get().libBoat);
            Get().lastSpawnedShip.transform.SetParent(Get().aiSpawnTransform);
        }
    }
}
