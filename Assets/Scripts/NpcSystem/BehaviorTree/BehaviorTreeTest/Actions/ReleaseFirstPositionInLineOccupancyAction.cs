using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTreeTest
{
    public class ReleaseFirstPositionInLineOccupancyAction : BTNode
    {
        ILinePositionManager _linePositionManager;

        public ReleaseFirstPositionInLineOccupancyAction(ILinePositionManager linePositionManager)
        {
            _linePositionManager = linePositionManager;
        }

        protected override NodeState OnUpdate()
        {
            if (_linePositionManager == null)
            {
                Debug.Log($"cant release first position, controller null");
                return NodeState.Failure;
            }
            _linePositionManager.SetPositionInLineOccupancy(0, false);
            return NodeState.Success;
        }
    }
}

