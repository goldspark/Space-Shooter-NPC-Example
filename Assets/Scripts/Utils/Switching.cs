using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Switching
    {


        public static void SwitchWeapon(Ship ship, int turretIndex, string weaponName)
        {
            Transform hardPoint = ship.mountedTurrets[turretIndex].transform.parent;
            GameObject.Destroy(ship.mountedTurrets[turretIndex].gameObject);
            ship.mountedTurrets[turretIndex] = GameObject.Instantiate(Spawns.Get().weaponSpawnFunctions[weaponName], hardPoint);
            ship.mountedTurrets[turretIndex].owner = ship;
        }

    }
}
