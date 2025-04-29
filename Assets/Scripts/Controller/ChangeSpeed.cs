using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class ChangeSpeed : ICommand<Ship, float>
    {
        public enum SpeedType
        {
            INCREASE,
            DECREASE
        }

        public SpeedType type = SpeedType.INCREASE;

        public void Exec(Ship entity, float data)
        {
         

            if (type == SpeedType.INCREASE)
            {
                entity.CurrentSpeed += entity.MaxSpeedIncreaseRate * Time.deltaTime;
            }
            else
            {
                entity.CurrentSpeed -= entity.MaxSpeedDecreaseRate * Time.deltaTime;
            }
        }
    }
}
