using UnityEngine;

namespace BehaviorTreeTest
{
    public class CheckKeyBoolValueCondition : BTNode
    {
        private string _key;
        private bool _targetValue;

        public CheckKeyBoolValueCondition(string key, bool targetValue)
        {
            this._key = key;
            this._targetValue = targetValue;
        }

        protected override NodeState OnUpdate()
        {
            bool? value = Blackboard.Get<bool?>(_key, null);
            if (value == null) 
            {
                return NodeState.Failure;
            }
            if (value == _targetValue)
            {
                return NodeState.Success;
            } 
            else
            {
                return NodeState.Failure;
            }
        }
    }
}