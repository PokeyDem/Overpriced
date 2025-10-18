
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class WarpNode : Node
{
    private NavMeshAgent _agent;
    private IHasTarget _context;

    public WarpNode(IHasTarget context, NavMeshAgent agent)
    {
        _context = context;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        _agent.Warp(_context.Target);
        return NodeState.SUCCESS;
    }
}
