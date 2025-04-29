using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Hardpoint : MonoBehaviour
    {
        public string assetPath;
        public bool isTurret = false;

        public void RemoveTurret()
        {
            assetPath = "";
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
