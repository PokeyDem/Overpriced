using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckBuyItemDecisionLeaf : Node
{
    private List<ItemData> _desiredItems;
    public CheckBuyItemDecisionLeaf(List<ItemData> desiredItems)
    {
        _desiredItems = desiredItems;
    }

    public override NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}
