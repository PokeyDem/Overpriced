using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTreeTest
{
    public class DecidePurchaseAction : BTNode
    {
        IHasDisplayTarget _displayTargetContext;
        List<ItemData> _desiredItems;
        IMoodController _moodController;

        public DecidePurchaseAction(IHasDisplayTarget displayTargetContext, List<ItemData> desiredItems, IMoodController moodController)
        {
            _displayTargetContext = displayTargetContext;
            _desiredItems = desiredItems;
            _moodController = moodController;
        }
        protected override NodeState OnUpdate()
        {
            DisplayContext displayTarget = _displayTargetContext.DisplayTarget;
            ItemData item = displayTarget.Info.ItemData;
            if (IsInterestedInBuying(item))
            {
                displayTarget.Flags.IsOccupied = true;
                displayTarget.Flags.IsChosen = true;
                _moodController.InvokeMoodChange(MoodType.Happy);
                Blackboard.Set("choseItemToBuy", true);
                return NodeState.Success;
                //GoToCheckout();
            }
            else
            {
                displayTarget.Flags.IsOccupied = false;
                _moodController.InvokeMoodChange(MoodType.Sad);
                return NodeState.Failure;
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
}

