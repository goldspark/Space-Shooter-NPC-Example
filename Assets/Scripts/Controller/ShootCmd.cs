using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Controller
{
    public class ShootCmd : ICommand<Turret, bool>
    {
        public void Exec(Turret entity, bool data)
        {
            entity.Shoot();
        }
    }
}
