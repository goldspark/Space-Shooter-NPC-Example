using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace Assets.Scripts.Controller
{
    public class AIController : SpaceShipController
    {
        [SerializeField]
        private Vector3 _shootTimeRange = new Vector3(1f, 2f);

        private Vector3 _target;

        private void Awake()
        {
            base.Awake();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {      
            base.FixedUpdate();
        }


    }
}
        