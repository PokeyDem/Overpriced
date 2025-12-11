using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTreeTest
{
    public class ChoosePositionInLineAsTargetAction : BTNode
    {
        List<Vector3> _counterPos;
        private int _currentPosInLine;
        IHasTarget _actor;
        ILinePositionManager _linePositionManager;

        public ChoosePositionInLineAsTargetAction(List<Vector3> counterPos, IHasTarget actor, ILinePositionManager linePositionManager)
        {
            _counterPos = counterPos;
            _currentPosInLine = -1;
            _actor = actor;
            _linePositionManager = linePositionManager;
        }

        protected override NodeState OnUpdate()
        {
            if (_currentPosInLine == -1)
            {
                if (!_linePositionManager.GetPositionInLineOccupancy(_counterPos.Count - 1))//last
                {
                    _linePositionManager.SetPositionInLineOccupancy(_counterPos.Count - 1, true);
                    _currentPosInLine = _counterPos.Count - 1;
                    _actor.Target = _counterPos[_counterPos.Count - 1];
                }
            }
            else if (_currentPosInLine != 0)
            {
                if (!_linePositionManager.GetPositionInLineOccupancy(_currentPosInLine - 1))
                {
                    _linePositionManager.SetPositionInLineOccupancy(_currentPosInLine, false);
                    _currentPosInLine--;
                    _linePositionManager.SetPositionInLineOccupancy(_currentPosInLine, true);
                }
                _actor.Target = _counterPos[_currentPosInLine];
            }

            if (_currentPosInLine == -1)
            {
                return NodeState.Failure;
            }
            string targetName = $"{_currentPosInLine}InLine";
            Blackboard.Set("hasTarget", true);
            Blackboard.Set("targetName", targetName);
            return NodeState.Success;
        }
    }
}

