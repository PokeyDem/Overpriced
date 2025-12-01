
using UnityEngine;
using BehaviorTreeTest;
using UnityEngine.AI;

public class WalkToTargetAction : BTNode
{
    private NavMeshAgent _agent;
    private Vector3 _target;
    public WalkToTargetAction(NavMeshAgent agent, Vector3 target)
    {
        _agent = agent;
        _target = target;
    }
    protected override void OnStart()
    {
        _agent.SetDestination(_target);
        Debug.Log($"Started walking to {_target}");
    }
    protected override NodeState OnUpdate()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            Debug.Log($"at {_target}");
            return NodeState.Success;
        }
        //Debug.Log($"Walking to {_target}, position: {_agent.transform.position}");
        return NodeState.Running;
    }
}
