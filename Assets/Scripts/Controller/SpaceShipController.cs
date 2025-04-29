using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controller
{
    public class SpaceShipController : MonoBehaviour
    {

        [HideInInspector]
        public Ship Entity;
        [HideInInspector]
        public Rigidbody rb;


        [HideInInspector]
        public float strafeValue = 0;
        
        /// <summary>
        /// Smooth the turning speed by this factor
        /// </summary>
        public float speedIncreaseFactor = 3;

        protected TurretLookAt _lookAt;
        protected ChangeSpeed _changeSpeedCmd;
        protected PitchCmd _pitchCmd;
        protected ShootCmd _shootCmd;
        protected NextTurretCmd _nextTurretCmd;
        protected RollCmd _rollCmd;
        protected StrafeCmd _strafeCmd;


        private const float STOP_DIST = 100.0f;

        protected void Awake()
        {
            _lookAt = new TurretLookAt();
            _changeSpeedCmd = new ChangeSpeed();
            _pitchCmd = new PitchCmd();
            _shootCmd = new ShootCmd();
            _nextTurretCmd = new NextTurretCmd();     
            _rollCmd = new RollCmd();
            _strafeCmd = new StrafeCmd();
            _pitchCmd.speedIncreaseFactor = speedIncreaseFactor;


        }


        private void Start()
        {

        }

        protected void FixedUpdate()
        {
            if (Entity.CurrentSpeed > Entity.MaxSpeed)
                Entity.CurrentSpeed = Entity.MaxSpeed;
            else if (Entity.CurrentSpeed < Entity.MinSpeed)
            {
                Entity.CurrentSpeed = Entity.MinSpeed;
            }

            rb.linearVelocity = Entity.CurrentSpeed * Entity.transform.forward + strafeValue * Entity.transform.right;
            if (rb.linearVelocity.sqrMagnitude > Entity.MaxSpeed * Entity.MaxSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * Entity.MaxSpeed;
        }

        /// <summary>
        /// Moves the vehicle towards the position
        /// </summary>
        /// <param name="dir"> Non Normalized Target to move to</param>
        public bool GoTo(Vector3 target)
        {

            Vector3 to = target - Entity.transform.position;
          
            LookAt(target);

            StartMoving(Entity.MaxSpeed);

            if (to.sqrMagnitude < STOP_DIST * STOP_DIST)
            {
                StopMoving();
                return true;
            }

            return false;
        }

        public bool GoTo(Vector3 target, float maxDist)
        {

            Vector3 to = target - Entity.transform.position;


            LookAt(target);

            StartMoving(Entity.MaxSpeed);

            if (to.sqrMagnitude < maxDist * maxDist)
            {
                StopMoving();
                return true;
            }

            return false;
        }

        public bool LookAt(Vector3 target)
        {
            Vector3 to = target - Entity.transform.position;
            float dot = Vector3.Dot(Entity.transform.forward, to.normalized);
            if (dot > 0.9982f)
                return true;

            Entity.transform.rotation = Quaternion.RotateTowards(Entity.transform.rotation, Quaternion.LookRotation(to, Entity.transform.up), Entity.TurnSpeed * Time.deltaTime);
                      
            return false;
        }

        public void Roll()
        {
            _rollCmd.Exec(Entity, true);
        }

        public void StartMoving(float targetSpeed)
        {
            _changeSpeedCmd.type = ChangeSpeed.SpeedType.INCREASE;
            _changeSpeedCmd.Exec(Entity, targetSpeed);
        }

        public void StartReversing(float targetSpeed)
        {
            _changeSpeedCmd.type = ChangeSpeed.SpeedType.DECREASE;
            _changeSpeedCmd.Exec(Entity, targetSpeed);
        }

        public bool StopMoving()
        {
            _changeSpeedCmd.type = ChangeSpeed.SpeedType.DECREASE;
            _changeSpeedCmd.Exec(Entity, 0);
            if (Entity.CurrentSpeed < 0)
            {
                Entity.CurrentSpeed = 0;
                return true;
            }

            return false;
        }

        public void NextGun()
        {
            _nextTurretCmd.Exec(Entity, 1);
        }
    }
}
