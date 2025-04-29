using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Controller
{
    public class NextTurretCmd : ICommand<Ship, int>
    {
        int _selectedIndex = 0;
        int _currTurret = 0;

        public void Exec(Ship entity, int data)
        {
            _currTurret = _selectedIndex++ % entity.Turrets.Count;
            entity.selectedTurret = entity.Turrets[_currTurret];
        }
    }
}
