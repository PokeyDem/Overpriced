using System.Collections.Generic;
using BehaviorTree;
public class LoopSequence : Node //Sequence that loops itself until last node(condition) returns SUCCESS
{
    private int _currentIndex = 0;
    public LoopSequence(List<Node> children) : base(children) { }
    public override NodeState Evaluate()
    {
        if (children.Count == 0)
            return NodeState.SUCCESS;
        while (_currentIndex < children.Count)
        {
            NodeState result = children[_currentIndex].Evaluate();
            switch (result)
            {
                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    return state;
                case NodeState.FAILURE:
                    state = NodeState.FAILURE;
                    _currentIndex = 0;
                    return state;
                case NodeState.RESTART:
                    _currentIndex = 0;
                    break;
                default://SUCCESS
                    _currentIndex++;
                    break;
            }
        }
        _currentIndex = 0;
        state = NodeState.SUCCESS;
        return state;

    }
}
