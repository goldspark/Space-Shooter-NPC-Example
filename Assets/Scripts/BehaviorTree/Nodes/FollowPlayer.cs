using Assets.Scripts.Entities;
using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class FollowPlayer : GoldNode
    {

        private static float MAX_DIST = 1000f;
        public FollowPlayer(GoldTreeBase tree) : base(tree)
        {

        }

        public override ReturnType Update(float delta)
        {

            if (Player.ship == null)
                return ReturnType.FAILURE;

            if (!Owner<Ship>().controller.GoTo(Player.ship.transform.position, MAX_DIST))
                return ReturnType.RUNNING;


            return ReturnType.SUCCESS;

        }
    }
}
