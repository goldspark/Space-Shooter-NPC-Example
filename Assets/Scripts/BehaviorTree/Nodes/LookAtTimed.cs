using Assets.Scripts.Controller;
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
    public class LookAtTimed : GoldNode
    {
        private string _targetKey;
        private float _minLookTime;
        private float _maxLookTime;
        private float _cT = 0f;
        private float _waitTime = 0f;

        public LookAtTimed(GoldTreeBase tree) : base(tree)
        {

        }


        public override void InitVarsFromLoader()
        {
            _targetKey = AILoader.LoadString(this, "entityKey");
            _minLookTime = AILoader.LoadFloat(this, "minLookTime");
            _maxLookTime = AILoader.LoadFloat(this, "maxLookTime");

            _waitTime = UnityEngine.Random.Range(_minLookTime, _maxLookTime);
        }


        public override ReturnType Update(float delta)
        {
            Vector3 target;
            if (_targetKey != null && _targetKey.Length > 0)
            {
                object entity = GetBB().GetEntity(_targetKey).Owner;

                if (entity == null || (entity as Ship).currentHp <= 0)
                    return ReturnType.FAILURE;

                target = (entity as Ship).transform.position;
            }
            else
                target = GetBB().GetVector("point");

            foreach (Turret t in Owner<Ship>().Turrets)
                t.AimAt(target);

            if (Owner<Ship>().controller.LookAt(target))
                return ReturnType.SUCCESS;
         
            _cT += delta;
            if(_cT > _waitTime)
            {
                _cT = 0f;
                _waitTime = UnityEngine.Random.Range(_minLookTime, _maxLookTime);
                return ReturnType.SUCCESS;
            }

            return ReturnType.RUNNING;
        }
    }
}
