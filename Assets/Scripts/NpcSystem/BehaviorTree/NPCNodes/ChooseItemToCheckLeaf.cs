using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ChooseItemToCheckLeaf : Node
{
    IHasDisplayChoices _displayChoicesContext;
    IHasTarget _targetContext;
    IHasDisplayTarget _displayTargetContext;
    IMoodController _moodController;

    public ChooseItemToCheckLeaf(IHasDisplayChoices displayChoicesContext, IHasTarget targetContext, IHasDisplayTarget displayTargetContext, IMoodController moodController)
    {
        _targetContext = targetContext;
        _displayTargetContext= displayTargetContext;
        _displayChoicesContext = displayChoicesContext;
        _moodController = moodController;
    }

    public override NodeState Evaluate()
    {
        List<DisplaySlotController> possibleDisplayChoices = new List<DisplaySlotController>(_displayChoicesContext.PossibleDisplayChoices);
        if (possibleDisplayChoices == null || possibleDisplayChoices.Count ==0)
        {
            _moodController.InvokeMoodChange(MoodType.Sad);
            state = NodeState.FAILURE;
            return state;
        }

        while (possibleDisplayChoices.Count > 0)
        {
            _displayTargetContext.DisplayTarget = null;
            DisplaySlotController displaySlotController = possibleDisplayChoices[0];
            possibleDisplayChoices.RemoveAt(0);
            ItemData item = displaySlotController.GetItem();
            if (item == null || displaySlotController == null)
            {
                continue;
            }
            if (displaySlotController.isChosen)
            {
                continue;
            }
            if (!displaySlotController.isOccupied)
            {
                displaySlotController.isOccupied = true;
            }
            else
            {
                if (possibleDisplayChoices.Find(ds => !ds.isOccupied) != null)
                {
                    possibleDisplayChoices.Add(displaySlotController);
                    continue;
                }
                else
                {
                    if (!displaySlotController.isOccupied || displaySlotController.isChosen)
                    {
                        
                    }
                    else
                    {
                        _targetContext.Target = displaySlotController.transform.position;
                        state = NodeState.RESTART;
                        return state;
                    }
                }
            }

            if (displaySlotController.isChosen)
            {
                continue;
            }
            _targetContext.Target=displaySlotController.transform.position;//SUCCESS
            _displayTargetContext.DisplayTarget = displaySlotController;
            _displayChoicesContext.PossibleDisplayChoices = possibleDisplayChoices;
            break;
        }
        if(_displayTargetContext.DisplayTarget==null)
        {
            _moodController.InvokeMoodChange(MoodType.Sad);
            state = NodeState.FAILURE;
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
