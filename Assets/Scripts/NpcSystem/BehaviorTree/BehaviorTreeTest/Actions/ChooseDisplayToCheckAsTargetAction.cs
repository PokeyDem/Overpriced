using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BehaviorTreeTest
{
    public class ChooseDisplayToCheckAsTargetAction : BTNode
    {
        IHasDisplayChoices _displayChoicesContext;
        IHasTarget _targetContext;
        IHasDisplayTarget _displayTargetContext;
        IMoodController _moodController;

        public ChooseDisplayToCheckAsTargetAction(IHasDisplayChoices displayChoicesContext, IHasTarget targetContext, IHasDisplayTarget displayTargetContext, IMoodController moodController)
        {
            _targetContext = targetContext;
            _displayTargetContext = displayTargetContext;
            _displayChoicesContext = displayChoicesContext;
            _moodController = moodController;
        }

        protected override NodeState OnUpdate()
        {
            var possibleDisplayChoices = new List<DisplayContext>(_displayChoicesContext.PossibleDisplayChoices ?? new List<DisplayContext>());

            if (possibleDisplayChoices.Count == 0)
            {
                _moodController.InvokeMoodChange(MoodType.Sad);
                Blackboard.Set("hagglingEnded", true);
                return NodeState.Failure;
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
                    _displayChoicesContext.PossibleDisplayChoices = new List<DisplayContext>(possibleDisplayChoices);

                    Blackboard.Set("hasTarget", true);
                    Blackboard.Set("targetName", "Display");
                    return NodeState.Success;
                }
                else
                {
                    possibleDisplayChoices.Add(displayContext);
                    _displayChoicesContext.PossibleDisplayChoices = new List<DisplayContext>(possibleDisplayChoices);
                    Blackboard.Set("hasTarget", false);
                    return NodeState.Running;
                }
            }
            _moodController.InvokeMoodChange(MoodType.Sad);
            Blackboard.Set("hagglingEnded", true);
            return NodeState.Failure;
        }
    }
}
