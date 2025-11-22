namespace BehaviorTreeTest
{
    public class Inverter : BTNode
    {
        private BTNode child;
        public Inverter(BTNode child) => this.child = child;


        protected override NodeState OnUpdate()
        {
            var r = child.Tick();
            return r switch
            {
                NodeState.Success => NodeState.Failure,
                NodeState.Failure => NodeState.Success,
                _ => NodeState.Running
            };
        }
    }


    public class Repeater : BTNode
    {
        private BTNode _child;
        public Repeater(BTNode child) => _child = child;


        protected override NodeState OnUpdate()
        {
            _child.Tick();
            return NodeState.Running;
        }
    }
}