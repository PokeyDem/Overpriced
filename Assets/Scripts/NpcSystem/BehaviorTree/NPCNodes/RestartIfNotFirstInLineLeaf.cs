using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class RestartIfNotFirstInLineLeaf : Node
{
    public NavMeshAgent _agent;
    public Vector3 _targetPosition;

    public RestartIfNotFirstInLineLeaf(NavMeshAgent agent, Vector3 targetPosition)
    {
        _agent = agent;
        _targetPosition=targetPosition;
    }
    public override NodeState Evaluate()
    {
        Vector2 currentPosition=new Vector2(_agent.transform.position.x, _agent.transform.position.z);
        Vector2 targetPosition = new Vector2(_targetPosition.x, _targetPosition.z);
        float distance = Vector2.Distance(currentPosition, targetPosition);
        Debug.Log(distance);
        if (distance<0.1f)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state=NodeState.RESTART;
        return state;
    }
}
