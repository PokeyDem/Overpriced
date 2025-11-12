using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        var possibleDisplayChoices = new List<DisplayContext>(_displayChoicesContext.PossibleDisplayChoices ?? new List<DisplayContext>());

        if (possibleDisplayChoices.Count == 0)
        {
            _moodController.InvokeMoodChange(MoodType.Sad);
            state = NodeState.FAILURE;
            return state;
        }

        _displayTargetContext.DisplayTarget = null;

        foreach (var displayContext in possibleDisplayChoices.ToList())
        {
            possibleDisplayChoices.Remove(displayContext);
            if (displayContext == null || displayContext.Info == null || displayContext.Info.ItemData == null)
                continue;

            if (displayContext.Flags.IsChosen)
                continue;

            if (!displayContext.Flags.IsOccupied)
            {
                displayContext.Flags.IsOccupied = true;
                _targetContext.Target = displayContext.Info.Position;
                _displayTargetContext.DisplayTarget = displayContext;
                _displayChoicesContext.PossibleDisplayChoices = possibleDisplayChoices;

                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                possibleDisplayChoices.Add(displayContext);
                _displayChoicesContext.PossibleDisplayChoices = possibleDisplayChoices;
                state = NodeState.RESTART;
                return state;
            }
        }

        _moodController.InvokeMoodChange(MoodType.Sad);
        state = NodeState.FAILURE;
        return state;
    }
}
