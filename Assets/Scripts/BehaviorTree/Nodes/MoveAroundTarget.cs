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
    public class MoveAroundTarget : GoldNode
    {
        float minRadius = 0;
        float maxRadius = 200;

        public MoveAroundTarget(GoldTreeBase tree) : base(tree)
        {
            
        }

        public override void InitVarsFromLoader()
        {
            minRadius = AILoader.LoadFloat(this, "minRadius");
            maxRadius = AILoader.LoadFloat(this, "maxRadius");

            if (!GetBB().Keys.Contains("point"))
                GetBB().Keys.AddKey("point");
        }

        public override ReturnType Update(float delta)
        {

            Ship targetShip = GetBB().GetEntity("target").Owner as Ship;
            if (targetShip == null)
                return ReturnType.FAILURE;

            Vector3 point = targetShip.transform.position + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(minRadius, maxRadius);
            GetBB().SetVector("point", point);

            return ReturnType.SUCCESS;
        }
    }
}
