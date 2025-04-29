using Assets.Scripts.Entities;
using Assets.Scripts.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Math
{
    public static class AimingUtils
    {
        public static float AimAhead(Ship owner, Ship target, Projectile projectile)
        {

            Vector3 toTarget = target.transform.position - owner.transform.position;
            Vector3 vr = target.controller.rb.linearVelocity - owner.controller.rb.linearVelocity;

            float a = Vector3.Dot(vr, vr) - (projectile.speed * projectile.speed);
            float b = 2f * Vector3.Dot(vr, toTarget);
            float c = Vector3.Dot(toTarget, toTarget);

            float det = b * b - 4f * a * c;

  
            if (det < 0f)
                return -1f;

            float sqrtDet = Mathf.Sqrt(det);
            float t1 = (-b + sqrtDet) / (2f * a);
            float t2 = (-b - sqrtDet) / (2f * a);

            if (t1 > 0f && t2 > 0f)
                return Mathf.Min(t1, t2);
            else if (t1 > 0f)
                return t1;
            else if (t2 > 0f)
                return t2;
            else
                return -1f; 
        }
    }
}
