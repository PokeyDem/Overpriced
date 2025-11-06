
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
        _agent.transform.LookAt(_context.Target + new Vector3(0, 0, -1));
        return NodeState.SUCCESS;
    }
}
