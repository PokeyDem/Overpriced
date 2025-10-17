using BehaviorTree;
using UnityEngine;

public class CheckIfShopHasItemsLeaf : Node
{

    public override NodeState Evaluate()
    {
        if (DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count != 0)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
