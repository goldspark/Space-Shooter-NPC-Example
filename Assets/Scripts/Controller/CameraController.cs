using Assets.Scripts.Entities;
using Assets.Scripts.Utils;
using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controller
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        public ParticleSystem spaceParticle;
        public GameObject objectToFollow;
        public float smoothRotation = 6f;
        public float positionSmoothSpeed = 3.0f;
        public Vector3 offset = new Vector3(0, 30, -70);

        private Ship _playerShip;

        private float _cT = 0f;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        { 
            Restart();
        }

        public void Restart()
        {
            if (objectToFollow != null)
                _playerShip = objectToFollow.GetComponent<Ship>();
        }

        void Update()
        {
            if(_playerShip != null)
            {
                var main = spaceParticle.main;
                float percent = _playerShip.CurrentSpeed / _playerShip.MaxSpeed;
                main.simulationSpeed = percent;

                if (_playerShip.CurrentSpeed < 1)
                    spaceParticle.gameObject.SetActive(false);
                else
                    spaceParticle.gameObject.SetActive(true);


            }

        }

        void FixedUpdate()
        {
            if(objectToFollow == null) return;

            Vector3 localOffset = objectToFollow.transform.right * offset.x + 
                objectToFollow.transform.up * offset.y + 
                objectToFollow.transform.forward * offset.z; // convert to coordinate space of the object
            Vector3 desiredPosition = objectToFollow.transform.position + localOffset; // translate in that space
            transform.position = Vector3.Slerp(transform.position, desiredPosition, Time.fixedDeltaTime * positionSmoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, objectToFollow.transform.rotation, Time.fixedDeltaTime * smoothRotation);
        }

        public void LookAtShip(Ship ship)
        {
            objectToFollow = ship.gameObject;
        }

        public void SetPlayerShip(Ship ship)
        {
            objectToFollow = ship.gameObject;
            ship.IsPlayer = true;
            Restart();
        }

    }
}
