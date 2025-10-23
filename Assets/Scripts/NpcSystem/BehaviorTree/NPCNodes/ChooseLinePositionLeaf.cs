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
    private int _targetPosInLine;

    public ChooseLinePositionLeaf(List<Vector3> counterPos, ILinePositionManager linePositionManager, IHasTarget actor)
    {
        _counterPos = counterPos;
        _currentPosInLine = -1;
        _actor = actor;
        _linePositionManager = linePositionManager;
    }

    public override NodeState Evaluate()
    {
        if (_currentPosInLine == -1)
        {
            _targetPosInLine=_counterPos.Count-1;
            if (!_linePositionManager.GetPositionInLineOccupancy(_counterPos.Count-1))//last
            {
                _linePositionManager.SetPositionInLineOccupancy(_counterPos.Count - 1, true);
                _currentPosInLine = _counterPos.Count - 1;
                _actor.Target = _counterPos[_counterPos.Count - 1];
            }
        }
        else if (_currentPosInLine !=0)
        {
            _targetPosInLine = _currentPosInLine - 1;
            if (!_linePositionManager.GetPositionInLineOccupancy(_currentPosInLine-1))
            {
                _linePositionManager.SetPositionInLineOccupancy(_currentPosInLine, false);
                _currentPosInLine--;
                _linePositionManager.SetPositionInLineOccupancy(_currentPosInLine, true);
            }
            _actor.Target = _counterPos[_currentPosInLine];
        }

        if (_currentPosInLine==-1)
        {
            state =NodeState.RESTART;
            return state;
        }
        state =NodeState.SUCCESS;
        return state;
    }
}
