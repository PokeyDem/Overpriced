
using BehaviorTree;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
public class StartWalkToTargetLeaf : Node
{
    private NavMeshAgent _agent;
    private IHasTarget _context;

    public StartWalkToTargetLeaf(IHasTarget context, NavMeshAgent agent)
    {
        _context = context;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        _agent.SetDestination(_context.Target);
        state = NodeState.SUCCESS;
        return state;

    }
}
