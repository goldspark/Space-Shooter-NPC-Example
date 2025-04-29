using Assets.Scripts.Entities;
using BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class TurretLookAt : ICommand<Turret, Vector3>
    {


        public void Exec(Turret entity, Vector3 data)
        {
            if(entity == null) return;

            entity.AimAt(data);
        }
    }
}
