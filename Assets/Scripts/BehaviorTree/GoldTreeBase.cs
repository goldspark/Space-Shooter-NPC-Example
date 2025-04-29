using BehaviorTree;
using SimpleBehaviorTreeEditor.Scripts.BehaviorTree;
using System.Collections.Generic;


namespace SimpleBehaviorTreeEditor.BehaviorTree
{

    /// <summary>
    /// NPCs should derive from this class as it's a base behavior tree
    /// class for NPC behavior.
    /// <example>
    /// Example:
    /// public class SentryTurretAI : GoldTreeBase{
    /// 
    /// }
    /// </example>
    ///
    /// </summary>
    public abstract class GoldTreeBase
    {
        public AIEntity owner;
        public GoldNode m_root;
        private Blackboard m_blackboard;

        public GoldTreeBase()
        {
            Init();
        }

        private void Init()
        {
            m_blackboard = new Blackboard();
            m_root = Start();
        }

        public abstract GoldNode Start();

        /// <summary>
        /// Add tree nodes 
        /// </summary>
        /// <param name="nodes">Add nodes</param>
        public void AddChildren(List<GoldNode> nodes)
        {
            foreach (GoldNode node in nodes)
            {
                m_root.children.Add(node);
            }
        }

        public void Update(float delta)
        {
            m_root.Update(delta);
        }

        public Blackboard GetBB()
        {
            return m_blackboard;
        }



    }
}
