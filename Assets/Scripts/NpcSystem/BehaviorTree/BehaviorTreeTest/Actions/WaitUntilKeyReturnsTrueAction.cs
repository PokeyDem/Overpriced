using UnityEngine;
namespace BehaviorTreeTest 
{
    public class WaitUntilKeyReturnsValueAction : BTNode
    {
        private string _key;
        private bool _value;

        public WaitUntilKeyReturnsValueAction(string key, bool value)
        {
            _key = key;
            _value = value;
        }

        protected override NodeState OnUpdate()
        {
            bool? value = Blackboard.Get<bool?>(_key, null);
            if (value == null)
            {
                return NodeState.Running;
            }
            if (value == _value)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Running;
            }
        }

    }
}