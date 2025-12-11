using UnityEngine;
using UnityEngine.AI;
namespace BehaviorTreeTest
{
    public class SetWalkTargetAction : BTNode
    {
        private IHasTarget _context;
        private Vector3 _target;
        private string _targetName;
        public SetWalkTargetAction(IHasTarget context, Vector3 target, string targetName)
        {
            _context = context;
            _target = target;
            _targetName = targetName;
        }
        protected override NodeState OnUpdate()
        {
            _context.Target = _target;
            Blackboard.Set("hasTarget", true);
            Blackboard.Set("targetName", _targetName);
            return NodeState.Success;
        }
    }
}
