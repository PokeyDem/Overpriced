using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeTest;

public class CheckShopHasItemsAction : BTNode
{
    protected override NodeState OnUpdate()
    {
        if (DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count == 0)
        {
            Blackboard.Set("shopEmpty", true);
        } else Blackboard.Set("shopEmpty", false);
        return NodeState.Success;
    }

}
