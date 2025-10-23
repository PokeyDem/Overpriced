using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ReleaseFirstPositionInLineOccupancy : Node
{
    ILinePositionManager _linePositionManager;

    public ReleaseFirstPositionInLineOccupancy(ILinePositionManager linePositionManager)
    {
        _linePositionManager = linePositionManager;
    }

    public override NodeState Evaluate()
    {
        if (_linePositionManager == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        _linePositionManager.SetPositionInLineOccupancy(0, false);
        state= NodeState.SUCCESS;
        return state;
    }
}

