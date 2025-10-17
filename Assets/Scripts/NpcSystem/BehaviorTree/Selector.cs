
using System.Collections.Generic;


namespace BehaviorTree
{
    public class Selector : Node
    {
        private int _currentIndex = 0;
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {

            while (_currentIndex < children.Count)
            {
                switch (children[_currentIndex].Evaluate())
                {
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.SUCCESS:
                        _currentIndex = 0;
                        state = NodeState.SUCCESS;
                        return state;
                    default://FAILURE
                        _currentIndex++;
                        return NodeState.RUNNING;
                }
            }
            _currentIndex = 0;
            state = NodeState.FAILURE;
            return state;

        }
    }
}

