using Assets.Scripts.Controller;
using Assets.Scripts.Entities;
using Assets.Scripts.Math;
using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class ShootAtEnemy : GoldNode
    {
        Ship target;
        float firingTime = 0f;
        float setFiringTime = 0f;
        float coolDownTime = 0f;
        float setCooldownTime = 0f;
        bool isFiring = false;

        float maxCooldownTime, maxFiringTime;
        bool randomized = true;
        public ShootAtEnemy(GoldTreeBase tree) : base(tree)
        {

        }

        public override void InitVarsFromLoader()
        {
            maxFiringTime = AILoader.LoadFloat(this, "maxFiringTime");
            maxCooldownTime = AILoader.LoadFloat(this, "maxCooldownTime");
            randomized = AILoader.LoadBool(this, "randomized");
        }

        public override ReturnType Update(float delta)
        {

            if (Owner<Ship>().Turrets.Count < 1)
                return ReturnType.SUCCESS;

            target = GetBB().GetEntity("target").Owner as Ship;
            if (target == null)
            {
                Owner<Ship>().curretTarget = null;
                return ReturnType.FAILURE;
            }


            Owner<Ship>().curretTarget = target;

            if (isFiring)
            {
                firingTime += delta;

                if (firingTime < setFiringTime)
                {
         
                    float t = AimingUtils.AimAhead(Owner<Ship>(), target, Owner<Ship>().selectedTurret.projectile);
                    Vector3 aimPoint = Vector3.zero;
                    if (t > 0)
                        aimPoint = target.transform.position + target.controller.rb.linearVelocity * t + UnityEngine.Random.insideUnitSphere * target.evade;
                    else
                        aimPoint = target.transform.position + UnityEngine.Random.insideUnitSphere * target.evade;

                    Owner<Ship>().selectedTurret.AimAt(aimPoint);
                    Owner<Ship>().selectedTurret.Shoot();

                   
                }
                else
                {
                    isFiring = false;
                    coolDownTime = 0f;
                    if (randomized)
                        setCooldownTime = UnityEngine.Random.Range(0f, maxCooldownTime);
                    else
                        setCooldownTime = maxCooldownTime;

                    Owner<Ship>().controller.NextGun();

                }
            }
            else
            {
                coolDownTime += delta;
                if (coolDownTime >= setCooldownTime)
                {
                    // Start firing again
                    isFiring = true;
                    firingTime = 0f;
                    if (randomized)
                        setFiringTime = UnityEngine.Random.Range(0f, maxFiringTime);
                    else
                        setFiringTime = maxFiringTime;

                }
            }

            return ReturnType.SUCCESS;
        }
    }
}
