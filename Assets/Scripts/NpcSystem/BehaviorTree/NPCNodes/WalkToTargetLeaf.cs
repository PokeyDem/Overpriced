
using BehaviorTree;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
public class WalkToTargetLeaf : Node
{
    private NavMeshAgent _agent;
    private IHasTarget _context;

    private bool _isWalking=false;
    public WalkToTargetLeaf(IHasTarget context, NavMeshAgent agent)
    {
        _context = context;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (!_isWalking)
        {
            _agent.SetDestination(_context.Target);
            _isWalking = true;
            state = NodeState.RUNNING;
            return state;
        }
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _isWalking = false;
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
