using BehaviorTreeTest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TransformAtPositionCondition : BTNode
{
    Transform _transform;
    Vector3 _target;

    public TransformAtPositionCondition(Transform transform, Vector3 target)
    {
        _transform = transform;
        _target = target;
    }

    protected override NodeState OnUpdate()
    {
        if (_transform != null) 
        {
            if (Vector3.Distance(_transform.position, _target) < 0.1f)
            {
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
