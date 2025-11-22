using UnityEngine;

namespace BehaviorTreeTest
{
    public class KeyReturnsTrueCondition : BTNode
    {
        private string _key;

        public KeyReturnsTrueCondition(string key)
        {
            this._key = key;
        }

        protected override NodeState OnUpdate()
        {
            bool? value = Blackboard.Get<bool?>(_key, null);
            if (value == null) 
            {
                Debug.Log($"{_key} is null");
                return NodeState.Failure;
            }
            if (value == true)
            {
                Debug.Log($"{_key} is true");
                return NodeState.Success;
            } 
            else
            {
                Debug.Log($"{_key} is false");
                return NodeState.Failure;
            }
        }
    }
}