using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class IsTargetSet : GoldNode
    {

        public IsTargetSet(GoldTreeBase tree) : base(tree)
        {

        }


        public override ReturnType Update(float delta)
        {
            if(GetBB().GetEntity("target") == null)
                return ReturnType.FAILURE;

            return ReturnType.SUCCESS;

        }
    }
}
