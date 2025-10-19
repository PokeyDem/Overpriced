using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DecidePurchaseLeaf : Node
{
    IHasDisplayTarget _displayTargetContext;
    List<ItemData> _desiredItems;
    IMoodController _moodController;

    public DecidePurchaseLeaf(IHasDisplayTarget displayTargetContext, List<ItemData> desiredItems, IMoodController moodController)
    {
        _displayTargetContext = displayTargetContext;
        _desiredItems = desiredItems;
        _moodController = moodController;
    }

    public override NodeState Evaluate()
    {
        DisplaySlotController displayTarget = _displayTargetContext.DisplayTarget;
        ItemData item = displayTarget.GetItem();
        if (IsInterestedInBuying(item))
        {
            displayTarget.isOccupied = true;
            displayTarget.isChosen = true;
            _moodController.InvokeMoodChange(MoodType.Happy);
            state=NodeState.SUCCESS;
            return state;
            //GoToCheckout();
        }
        else
        {
            displayTarget.isOccupied = false;
            _moodController.InvokeMoodChange(MoodType.Sad);
            state = NodeState.RESTART;
            return state;
            //Debug.Log($"Doesnt want {item.Name}"); 
        }
    }
    private bool IsInterestedInBuying(ItemData item)
    {
        bool isInterested = false;
        int chanceToBuy = 0;
        if (_desiredItems.Exists(i => i.ID == item.ID))
        {
            chanceToBuy = 90;//Math.Max(_minChanceToBuy,60);
        }
        else chanceToBuy = 10;
        int random = UnityEngine.Random.Range(0, 100);
        if (random < chanceToBuy)
        {
            isInterested = true;
        }
        else isInterested = false;
        return isInterested;
    }
}
