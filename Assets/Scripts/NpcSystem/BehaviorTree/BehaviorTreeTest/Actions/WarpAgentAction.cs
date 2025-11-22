using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeTest;
using UnityEngine.AI;

public class WarpAgentAction : BTNode
{
    private NavMeshAgent _agent;
    private Vector3 _target;

    public WarpAgentAction(NavMeshAgent agent, Vector3 target)
    {
        _agent = agent;
        _target = target;
    }

    protected override NodeState OnUpdate()
    {
        _agent.Warp(_target);
        _agent.transform.LookAt(_target + new Vector3(0, 0, -1));
        return NodeState.Success;
    }
}
