
using UnityEngine;

namespace BehaviorTreeTest
{
    public class WaitAction : BTNode
    {
        private float _duration;

        private float _startTime;

        public WaitAction(float duration)
        {
            _duration = duration;
        }

        protected override void OnStart()
        {
            Blackboard.Set("isWaiting", true);
            _startTime = Time.time;
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time - _startTime >= _duration)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        protected override void OnStop()
        {
            Blackboard.Set("isWaiting", false);
        }
    }
}