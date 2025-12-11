using UnityEngine;
namespace BehaviorTreeTest
{
    public class SetKeyAction<T> : BTNode
    {
        private string _key;
        private object _value;

        public SetKeyAction(string key, T value)
        {
            _key = key;
            _value = value;
        }

        protected override NodeState OnUpdate()
        {
            Blackboard.Set(_key, _value);
            return NodeState.Success;
        }

    }
}
