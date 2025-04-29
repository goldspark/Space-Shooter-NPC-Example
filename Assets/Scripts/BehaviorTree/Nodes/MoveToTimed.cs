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
    public class MoveToTimed : GoldNode
    {

        bool _isDynamicEntity = false;
        string _entityKey;
        float _moveTime = 10f;


        private float _cT = 0f;
        public MoveToTimed(GoldTreeBase tree) : base(tree)
        {

        }

        public override void InitVarsFromLoader()
        {
            _isDynamicEntity = AILoader.LoadBool(this, "isDynamicEntity");
            _entityKey = AILoader.LoadString(this, "entityKey");
            _moveTime = AILoader.LoadFloat(this, "moveTime");
        }

        public override ReturnType Update(float delta)
        {  

            if (!_isDynamicEntity)
            {
                Vector3 point = GetBB().GetVector("point");

                if (Owner<Ship>().controller.GoTo(point))
                {
                    _cT = 0f;
                    return ReturnType.SUCCESS;
                }
            }
            else
            {
                Vector3 point = (GetBB().GetEntity(_entityKey).Owner as Ship).transform.position;

                if (Owner<Ship>().controller.GoTo(point))
                {
                    _cT = 0f;
                    return ReturnType.SUCCESS;
                }
            }


            _cT += delta;
            if(_cT > _moveTime)
            {
                _cT = 0f;
                return ReturnType.SUCCESS;
            }

            return ReturnType.RUNNING;
        }
    }
}
