using System.Collections.Generic;
namespace BehaviorTreeTest
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    public abstract class BTNode
    {
        public BTNode Parent { get; private set; }
        public List<BTNode> Children { get; private set; } = new List<BTNode>();


        protected Blackboard Blackboard;
        private Dictionary<string, object> localMemory = new Dictionary<string, object>();

        private bool started;


        public void Initialize(BTNode parent, Blackboard bb)
        {
            Parent = parent;
            Blackboard = bb;
        }


        public NodeState Tick()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }


            var result = OnUpdate();


            if (result != NodeState.Running)
            {
                OnStop();
                started = false;
            }


            return result;
        }


        protected virtual void OnStart() { }
        protected virtual void OnStop() { }
        protected abstract NodeState OnUpdate();


        protected void SetLocal<T>(string key, T value) => localMemory[key] = value;
        protected T GetLocal<T>(string key, T defaultValue = default)
        {
            if (localMemory.TryGetValue(key, out var obj) && obj is T cast)
                return cast;
            return defaultValue;
        }


        protected void SetGlobal<T>(string key, T value) => Blackboard.Set(key, value);
        protected T GetGlobal<T>(string key, T defaultValue = default) => Blackboard.Get(key, defaultValue);
    }
}