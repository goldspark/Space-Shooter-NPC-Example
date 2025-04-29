using Assets.Scripts.Controller;
using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class MoveForSeconds : GoldNode
    {

        private float _moveTime = 5.0f;
        private float _cT = 0f;

        public MoveForSeconds(GoldTreeBase tree) : base(tree)
        {

        }

        public override void InitVarsFromLoader()
        {

        }

        public override ReturnType Update(float delta)
        {

            _cT += delta;
            if (_cT > _moveTime)
            {
                _cT = 0.0f;
                Owner<Entities.Ship>().controller.StopMoving();
                return ReturnType.SUCCESS;
            }

            Owner<Entities.Ship>().controller.StartMoving(Owner<Entities.Ship>().MaxSpeed);

            return ReturnType.RUNNING;
        }
    }
}
