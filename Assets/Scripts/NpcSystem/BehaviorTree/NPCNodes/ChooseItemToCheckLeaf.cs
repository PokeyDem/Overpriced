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
        List<DisplayContext> possibleDisplayChoices = new List<DisplayContext>(_displayChoicesContext.PossibleDisplayChoices);
        if (possibleDisplayChoices == null || possibleDisplayChoices.Count ==0)
        {
            _moodController.InvokeMoodChange(MoodType.Sad);
            state = NodeState.FAILURE;
            return state;
        }

        while (possibleDisplayChoices.Count > 0)
        {
            _displayTargetContext.DisplayTarget = null;
            DisplayContext displayContext = possibleDisplayChoices[0];
            possibleDisplayChoices.RemoveAt(0);
            ItemData item = displayContext.Info.ItemData;
            if (item == null || displayContext == null)
            {
                continue;
            }
            if (displayContext.Flags.IsChosen)
            {
                continue;
            }
            if (!displayContext.Flags.IsOccupied)
            {
                displayContext.Flags.IsOccupied = true;
            }
            else
            {
                if (possibleDisplayChoices.Find(ds => !ds.Flags.IsOccupied) != null)
                {
                    possibleDisplayChoices.Add(displayContext);
                    continue;
                }
                else
                {
                    if (!displayContext.Flags.IsOccupied || displayContext.Flags.IsChosen)
                    {
                        
                    }
                    else
                    {
                        _targetContext.Target = displayContext.Info.Position;
                        state = NodeState.RESTART;
                        return state;
                    }
                }
            }

            if (displayContext.Flags.IsChosen)
            {
                continue;
            }
            _targetContext.Target=displayContext.Info.Position;//SUCCESS
            _displayTargetContext.DisplayTarget = displayContext;
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
