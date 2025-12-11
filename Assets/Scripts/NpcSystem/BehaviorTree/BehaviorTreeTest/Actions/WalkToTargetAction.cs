
using UnityEngine;
using BehaviorTreeTest;
using UnityEngine.AI;

public class WalkToTargetAction : BTNode
{
    private NavMeshAgent _agent;
    private Vector3 _target;
    private IHasTarget _targetContext;
    public WalkToTargetAction(NavMeshAgent agent, Vector3 target)
    {
        _agent = agent;
        _target = target;
    }
    public WalkToTargetAction(NavMeshAgent agent, IHasTarget targetContext)
    {
        _agent = agent;
        _targetContext = targetContext;
    }
    protected override void OnStart()
    {
        if (_targetContext != null)
        {
            _target = _targetContext.Target;
        }
        _agent.SetDestination(_target);
    }
    protected override NodeState OnUpdate()
    {
        if (_targetContext != null)
        {
            if( _targetContext.Target != _target)
            {
                OnStart();
            }
        }
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}
