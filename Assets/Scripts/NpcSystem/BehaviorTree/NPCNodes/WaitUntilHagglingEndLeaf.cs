using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUntilHagglingEndLeaf : Node
{
    IHaggler _actor;
    public WaitUntilHagglingEndLeaf(IHaggler actor)
    {
        _actor = actor;
    }
    public override NodeState Evaluate()
    {
        if(_actor == null )
        {
            state= NodeState.FAILURE;
            return state;
        } 

        if(_actor.IsHaggling) 
        {
            state = NodeState.RUNNING;
            return state;
        } else
        {
            state = NodeState.SUCCESS;
            return state;
        }
    }
}
