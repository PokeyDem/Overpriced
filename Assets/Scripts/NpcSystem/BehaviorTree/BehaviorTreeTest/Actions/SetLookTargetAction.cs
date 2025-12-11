using UnityEngine;
using UnityEngine.AI;
namespace BehaviorTreeTest
{
    public class SetLookTargetAction : BTNode
    {
        private IHasTarget _context;
        private Vector3 _target;
        public SetLookTargetAction(IHasTarget context, Vector3 target)
        {
            _context = context;
            _target = target;
        }
        protected override NodeState OnUpdate()
        {
            _context.Target = _target;
            return NodeState.Success;
        }
    }
}