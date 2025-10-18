using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHagglingLeaf : Node
{
    IHaggler _actor;

    public StartHagglingLeaf(IHaggler actor)
    {
        _actor = actor;
    }
    public override NodeState Evaluate()
    {
        _actor.IsHaggling = true;
        state = NodeState.SUCCESS;
        return state;
    }
}
