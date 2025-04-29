using Assets.Scripts.BehaviorTree.AIEntities;
using Assets.Scripts.Math;
using Assets.Scripts.Projectiles;
using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Entities
{
    public class Turret : MonoBehaviour
    {
        public string Name;
        public string assetPath;
        public Projectile projectile;

        public bool isTurret = true;

        [SerializeField]
        private Transform _gun, _turretBase;

        [SerializeField]
        private Transform _projectileSpawnLoc;

        public Ship owner;

        public Vector2 maxMinHorizontalRot = new Vector2(360, 360);
        public Vector2 maxMinVerticalRot = new Vector2(90, 20);

        public float RotateSpeed = 100.0f;

        public float rateOfFire = 0.25f;
        private float _cT = 0.0f;
        private bool _isReadyToFire = false;
        [HideInInspector]
        public bool isLineOfSight = false;

        private float _autoAimCt = 0.0f;
        private float _autoAimWait = 1.0f;
        private float _autoAimTargetCt = 0.0f;
        private float _autoAimTargetWait = 1.0f;
        private Vector3 _autoTurretTarget = Vector3.zero;
        private int _autoTurretTargetIndex = -1;

        private bool _autoAimPeriodicWait = false;

        private void Start()
        {
            _cT = rateOfFire + 0.1f;
        }

        private void Update()
        {
            if (isTurret)
                AutoTurret();

            if (!_isReadyToFire)
            {
                _cT += Time.deltaTime;

                if (_cT > rateOfFire)
                {
                    _cT = 0.0f;
                    _isReadyToFire = true;
                }
            }
        }

        private void AutoTurret()
        {
            if (owner == null || owner.detectedShips.Count < 1)
                return;


            //Next target selection
            _autoAimTargetCt += Time.deltaTime;
            if (_autoAimTargetCt > _autoAimTargetWait)
            {
                _autoAimTargetWait = UnityEngine.Random.Range(5f, 15f);
                _autoAimTargetCt = 0.0f;
                _autoTurretTargetIndex++;
                if (_autoTurretTargetIndex >= owner.detectedShips.Count)
                    _autoTurretTargetIndex = 0;
            }

            if (_autoTurretTargetIndex < 0)
                return;

            //Periodic shooting
            _autoAimCt += Time.deltaTime;

            if (_autoAimPeriodicWait && _autoAimCt < _autoAimWait)
            {
                return;
            }
            else if(_autoAimPeriodicWait)
            {
                _autoAimWait = UnityEngine.Random.Range(0.2f, 3.0f);
                _autoAimPeriodicWait = false;
                _autoAimCt = 0.0f;
            }

            if (_autoAimCt < _autoAimWait)
            {
                if(_autoTurretTargetIndex >= owner.detectedShips.Count)
                    _autoTurretTargetIndex = 0;
                var id = owner.detectedShips[ _autoTurretTargetIndex ];
                var ship = (Ship)AIEntityManager.Get().GetEntity(id).Owner;

                float t = AimingUtils.AimAhead(owner, ship, projectile);
                Vector3 aimPoint = Vector3.zero;
                if (t > 0)
                    aimPoint = ship.transform.position + ship.controller.rb.linearVelocity * t + UnityEngine.Random.insideUnitSphere * 10f;
                else
                    aimPoint = ship.transform.position + UnityEngine.Random.insideUnitSphere * 10f;

                AimAt(aimPoint);
                Shoot();
            }
            else
            {
                _autoAimPeriodicWait = true;
                _autoAimWait = UnityEngine.Random.Range(0.1f, 4.0f);
                _autoAimCt = 0.0f;
            }



        }

        private void TurretBaseRotation(Vector3 target)
        {
            // Get aim position in parent gameobject local space in relation to aim position world space 
            Vector3 targetPositionInLocalSpace = transform.parent.InverseTransformPoint(target);
            // Set "aimPoint" Y position to zero, since this is horizontal rotation n because we dont need it
            targetPositionInLocalSpace.y = 0.0f;
            // Store limit value of the rotation
            Vector3 limitedRotation = targetPositionInLocalSpace;

            // limit turret horizontal rotation according to its rotation limit, so how much to the left and how much to the right can it rotate
            // Rotate vector around a Cross(Vector3.forard, targetPosInLocalSpace)
            Vector3 localSpace = _turretBase.InverseTransformPoint(target);
            if (localSpace.x >= 0.0f)
                limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * maxMinHorizontalRot.x, float.MaxValue);
            else
                limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * maxMinHorizontalRot.y, float.MaxValue);

            Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
            // Rotate the turret
            _turretBase.transform.localRotation = Quaternion.RotateTowards(_turretBase.transform.localRotation, whereToRotate, RotateSpeed * Time.deltaTime);

        }

        private void TurretGunRotation(Vector3 target)
        {
            // Get aim position in barrel gameobject local space in relation to aim position world space 
            Vector3 targetPositionInLocalSpace = _turretBase.transform.InverseTransformPoint(target);

            // Set "TargetPositionInLocalSpace" X position to zero, since this is vertical rotation n because we dont need it
            targetPositionInLocalSpace.x = 0.0f;
            // Store limit value of the rotation
            Vector3 limitedRotation = targetPositionInLocalSpace;
            // limit turret vertical rotation according to its rotation limit, so how much up and down can it rotate
            if (targetPositionInLocalSpace.y >= 0.0f)
                limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * maxMinVerticalRot.x, float.MaxValue);
            else
                limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, Mathf.Deg2Rad * maxMinVerticalRot.y, float.MaxValue);

            //Get direction
            Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
            // Rotate the barrel
            _gun.transform.localRotation = Quaternion.RotateTowards(_gun.localRotation, whereToRotate, RotateSpeed * Time.deltaTime);

            Vector3 fromTo = (target - _gun.transform.position).normalized;
            if (Vector3.Dot(fromTo, _gun.transform.forward) < 0.998f)
            {
                isLineOfSight = false;
            }
            else
                isLineOfSight = true;

        }

        public void AimAt(Vector3 target)
        {
            TurretBaseRotation(target);
            TurretGunRotation(target);
        }

        public void Shoot()
        {
            if (!_isReadyToFire || !isLineOfSight)
                return;
            _isReadyToFire = false;

            

            var proj = Instantiate(projectile, _projectileSpawnLoc.position, _gun.rotation);
            proj.spawnLocation = _projectileSpawnLoc;
            Physics.IgnoreCollision(proj.GetComponent<Collider>(), owner.GetComponent<Collider>());
            proj.owner = owner;
        }



      

      

    }
}
