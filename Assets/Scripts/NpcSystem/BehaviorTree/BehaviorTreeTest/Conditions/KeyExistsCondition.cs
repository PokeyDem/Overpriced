
namespace BehaviorTreeTest
{
    public class KeyExistsCondition : BTNode
    {
        private string _key;

        public KeyExistsCondition(string key)
        {
            this._key = key;
        }

        protected override NodeState OnUpdate()
        {
            if (Blackboard.Has(_key))
                return NodeState.Success;
            else
                return NodeState.Failure;
        }
    }
}