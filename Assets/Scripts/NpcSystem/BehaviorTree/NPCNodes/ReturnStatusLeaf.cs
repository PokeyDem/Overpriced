using BehaviorTree;

public class ReturnStatusLeaf : Node
{
    NodeState _status;

    public ReturnStatusLeaf(NodeState status)
    {
        _status=status;
    }

    public override NodeState Evaluate()
    {
        state=_status;
        return state;
    }
}
