using Assets.Scripts.Controller;
using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [HideInInspector]
        public Ship ship;

        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }

        private void Update()
        {
            if(ship == null) return;
            _slider.value = ship.currentHp/ship.maxHp;
        }

        

    }


}
