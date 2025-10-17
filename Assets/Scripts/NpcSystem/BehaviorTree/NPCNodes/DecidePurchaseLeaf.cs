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
    IHasItemToBuy _itemToBuyContext;

    public DecidePurchaseLeaf(IHasDisplayTarget displayTargetContext, List<ItemData> desiredItems,IHasItemToBuy itemToBuyContext, IMoodController moodController)
    {
        _displayTargetContext = displayTargetContext;
        _desiredItems = desiredItems;
        _itemToBuyContext = itemToBuyContext;
        _moodController = moodController;
    }

    public override NodeState Evaluate()
    {
        DisplaySlotController displayTarget = _displayTargetContext.DisplayTarget;
        ItemData item = displayTarget.GetItem();
        if (IsInterestedInBuying(item))
        {
            //Debug.Log($"Wants {item.Name}");
            _itemToBuyContext.ItemToBuy = item;
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
        Debug.Log(_desiredItems.Exists(i => i.ID == item.ID));
        Debug.Log(item);
        if (_desiredItems.Exists(i => i.ID == item.ID))
        {
            chanceToBuy = 90;//Math.Max(_minChanceToBuy,60);
        }
        else chanceToBuy = 10;
        int random = UnityEngine.Random.Range(0, 100);
        //Debug.Log($"Chance to buy: {chanceToBuy}, random: {random}");
        if (random < chanceToBuy)
        {
            isInterested = true;
        }
        else isInterested = false;
        Debug.Log($"{isInterested} {random} {chanceToBuy}");
        return isInterested;
    }
}
