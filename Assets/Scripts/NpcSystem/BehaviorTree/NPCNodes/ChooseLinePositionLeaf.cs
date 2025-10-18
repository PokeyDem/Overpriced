using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ChooseLinePositionLeaf : Node
{
    List<Vector3> _counterPos;
    private int _currentPosInLine;
    IHasTarget _actor;
    ILinePositionManager _linePositionManager;

    public ChooseLinePositionLeaf(List<Vector3> counterPos, ILinePositionManager linePositionManager, IHasTarget actor)
    {
        _counterPos = counterPos;
        _currentPosInLine = -1;
        _actor = actor;
        _linePositionManager = linePositionManager;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i < _counterPos.Count; i++)
        {
            if (_linePositionManager.GetPositionInLineOccupancy(i))
            {
                if(_currentPosInLine==i)
                {
                    _actor.Target = _counterPos[i];
                    break;
                } else
                    continue;
            }
            else
            {
                if (_currentPosInLine >= 0)
                {
                    _linePositionManager.SetPositionInLineOccupancy(_currentPosInLine, false);
                }
                _linePositionManager.SetPositionInLineOccupancy(i,true);
                _currentPosInLine = i;
                _actor.Target = _counterPos[i];
                break;
            }
        }
        if(_currentPosInLine==-1)
        {
            state =NodeState.RESTART;
            return state;
        }
        state =NodeState.SUCCESS;
        return state;
    }
}
