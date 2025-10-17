using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SetTargetLeaf : Node
{
    IHasTarget _actor;
    private Vector3 _target;

    public SetTargetLeaf(IHasTarget actor, Vector3 target)
    {
        _actor = actor;
        _target = target;
    }

    public override NodeState Evaluate()
    {
        _actor.Target = _target;
        return NodeState.SUCCESS;
    }
}
