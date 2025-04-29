using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class PitchCmd : ICommand<Ship, bool>
    {

        public enum PitchType
        {
            UP,
            DOWN
        };

        public PitchType type = PitchType.UP;
        public float speedIncreaseFactor = 3;
        

        float currentSpeed = 0.0f;
        public void Exec(Ship entity, bool data)
        {
            if (data)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, entity.TurnSpeed, Time.deltaTime * speedIncreaseFactor);
                float modifier = 1.0f;

                if (type == PitchType.DOWN)
                    modifier = -1.0f;
 
                entity.transform.Rotate(entity.transform.right * (modifier * currentSpeed) * Time.deltaTime, Space.World);
            }
            else
               currentSpeed = 0.0f;
        }
    }
}
