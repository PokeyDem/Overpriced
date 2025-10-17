
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        private int _currentIndex = 0;
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            if (children.Count == 0)
                return NodeState.SUCCESS;

            while (_currentIndex < children.Count)
            {
                switch (children[_currentIndex].Evaluate())
                {
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.FAILURE:
                        _currentIndex = 0;
                        state = NodeState.FAILURE;
                        return state;
                    default://SUCCESS
                        _currentIndex++;
                        return _currentIndex == children.Count ? NodeState.SUCCESS : NodeState.RUNNING;
                }
            }
            _currentIndex = 0;
            state = NodeState.SUCCESS;
            return state;
        }
    }
}

