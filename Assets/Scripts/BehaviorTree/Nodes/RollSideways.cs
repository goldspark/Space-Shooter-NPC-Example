using Assets.Scripts.Controller;
using Assets.Scripts.Entities;
using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class RollSideways : GoldNode
    {
        public RollSideways(GoldTreeBase tree) : base(tree)
        {

        }

        public override ReturnType Update(float delta)
        {
            Owner<Ship>().controller.Roll();
            return ReturnType.SUCCESS;
        }
    }
}
