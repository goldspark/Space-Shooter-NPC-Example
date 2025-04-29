using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{

    public class StrafeCmd : ICommand<SpaceShipController, float>
    {


        public void Exec(SpaceShipController controller, float data)
        {
            controller.strafeValue += data * Time.deltaTime;

            float max = (controller.Entity.MaxSpeed * 0.4f);

            if (controller.strafeValue > max)
                controller.strafeValue = max;
            else if(controller.strafeValue < -max)
                controller.strafeValue = -max;

        }
    }
}
