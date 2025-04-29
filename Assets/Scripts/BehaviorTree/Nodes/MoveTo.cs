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
    public class MoveTo : GoldNode
    {

        bool _isDynamicEntity = false;
        string _entityKey;

        public MoveTo(GoldTreeBase tree) : base(tree)
        {

        }

        public override void InitVarsFromLoader()
        {
            _isDynamicEntity = AILoader.LoadBool(this, "isDynamicEntity");
            _entityKey = AILoader.LoadString(this, "entityKey");
        }

        public override ReturnType Update(float delta)
        {
            if (!_isDynamicEntity)
            {
                Vector3 point = GetBB().GetVector("point");

                if (Owner<Ship>().controller.GoTo(point))
                    return ReturnType.SUCCESS;
            }
            else
            {
                Vector3 point = (GetBB().GetEntity(_entityKey).Owner as Ship).transform.position;

                if (Owner<Ship>().controller.GoTo(point))
                    return ReturnType.SUCCESS;  
            }

            return ReturnType.RUNNING;
        }
    }
}
