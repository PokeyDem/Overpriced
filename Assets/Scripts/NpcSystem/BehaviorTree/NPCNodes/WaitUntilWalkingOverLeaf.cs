using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitUntilWalkingOverLeaf : Node
{
    NavMeshAgent _agent;

    public WaitUntilWalkingOverLeaf(NavMeshAgent agent)
    {
        _agent = agent;
    }
    public override NodeState Evaluate()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
