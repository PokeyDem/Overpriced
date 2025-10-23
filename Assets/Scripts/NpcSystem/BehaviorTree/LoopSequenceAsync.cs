using System.Collections.Generic;
using BehaviorTree;
public class LoopSequenceAsync : Node //Sequence that loops when Restart returned
{
    private int _currentIndex = 0;
    public LoopSequenceAsync(List<Node> children) : base(children) { }
    public override NodeState Evaluate()
    {
        if (children.Count == 0)
            return NodeState.SUCCESS;
        while (_currentIndex < children.Count)
        {
            NodeState result = children[_currentIndex].Evaluate();
            switch (result)
            {
                case NodeState.FAILURE:
                    state = NodeState.FAILURE;
                    _currentIndex = 0;
                    return state;
                case NodeState.RESTART:
                    _currentIndex = 0;
                    break;
                default://SUCCESS or RUNNING
                    _currentIndex++;
                    break;
            }
        }
        _currentIndex = 0;
        state = NodeState.SUCCESS;
        return state;

    }
}
