using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class RollCmd : ICommand<Ship, bool>
    {
        public void Exec(Ship entity, bool data)
        {
            if (data)
                entity.transform.Rotate(entity.transform.forward * -(entity.TurnSpeed * 2.0f) * Time.deltaTime, Space.World);
        }
    }
}
