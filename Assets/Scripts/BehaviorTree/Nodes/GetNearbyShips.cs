using Assets.Scripts.Entities;
using BehaviorTree;
using SimpleBehaviorTreeEditor.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree.Nodes
{
    public class GetNearbyShips : GoldNode
    {
        private Ship _owner;
        private string _keyName;

        public GetNearbyShips(GoldTreeBase tree) : base(tree)
        {

        }

        public override void InitVarsFromLoader()
        {
            _keyName = AILoader.LoadString(this, "keyName");
            if (!GetBB().Keys.Contains(_keyName))
                GetBB().Keys.AddKey(_keyName);
        }

        public override ReturnType Update(float delta)
        {
            _owner = Owner<Ship>();

            if (_owner.detectedShips == null || _owner.detectedShips.Count == 0)    
                return ReturnType.FAILURE;        

            int id = UnityEngine.Random.Range(0, _owner.detectedShips.Count);
            GetBB().SetEntity(_keyName, AIEntityManager.Get().GetEntity(_owner.detectedShips[id]));

            return ReturnType.SUCCESS;
        }
    }
}
