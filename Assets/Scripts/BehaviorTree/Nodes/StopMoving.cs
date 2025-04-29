using Assets.Scripts.Controller;
using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class StopMoving : GoldNode
    {
        public StopMoving(GoldTreeBase tree) : base(tree)
        {

        }

        public override ReturnType Update(float delta)
        {
            if(!Owner<Entities.Ship>().controller.StopMoving())
                return ReturnType.RUNNING;
            return ReturnType.SUCCESS;
        }
    }
}
