using UnityEngine;
namespace BehaviorTreeTest
{
    public class SetWaitTimeAction : BTNode
    {
        private float _duration;

        public SetWaitTimeAction(float duration)
        {
            _duration = duration;
        }
        protected override NodeState OnUpdate()
        {
            Blackboard.Set("waitTime", _duration);
            Blackboard.Set("hasWaitTime", true);
            return NodeState.Success;
        }
    }
}
