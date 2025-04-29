using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class Random : GoldNode
    {

        private float _percent = 50;
        private int timesHit = 0;
        private const int TOTAL_TIMES = 2;
        
        public Random(GoldTreeBase tree) : base(tree)
        {
        }

        public override void InitVarsFromLoader()
        {
            _percent = AILoader.LoadFloat(this, "percent");
        }

        public override ReturnType Update(float delta)
        {

            float num = UnityEngine.Random.Range(0.0f, 100.0f);

            if (timesHit > TOTAL_TIMES)
            {
                timesHit = 0;
                return ReturnType.FAILURE;
            }

            if (num <= _percent)
            {
                timesHit++;
                return ReturnType.SUCCESS;
            }

            return ReturnType.FAILURE;
        }
    }
}
