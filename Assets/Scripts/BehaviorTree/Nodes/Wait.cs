using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Scripts.AI.BehaviorTree.Nodes
{
    public class Wait : GoldNode
    {
        float _waitTime;
        float _maxWaitTime = 2.0f;
        float _minWaitTime = 0.0f;

        bool _randomized = false;
        public Wait(GoldTreeBase tree) : base(tree)
        {

        }

        public override void InitVarsFromLoader()
        {
            _waitTime =  AILoader.LoadFloat(this, "waitTime");
            _maxWaitTime = AILoader.LoadFloat(this, "maxWaitTime");
            _minWaitTime = AILoader.LoadFloat(this, "minWaitTime");
            _randomized = AILoader.LoadBool(this, "randomized");
        }

        public override ReturnType Update(float delta)
        {

            _waitTime -= delta;
            if (!_randomized && _waitTime < 0)
            {
                _waitTime = _maxWaitTime;
                return ReturnType.SUCCESS;
            }
            else if (_randomized && _waitTime < 0)
            {
                _waitTime = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
                return ReturnType.SUCCESS;
            }
        
            return ReturnType.RUNNING;
        }

    }
}
