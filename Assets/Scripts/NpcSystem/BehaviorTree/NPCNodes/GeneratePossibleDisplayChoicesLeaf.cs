using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;

public class GeneratePossibleDisplayChoicesLeaf : Node
{
    IHasDisplayChoices _context;
    public GeneratePossibleDisplayChoicesLeaf(IHasDisplayChoices context)
    {
        _context = context;
    }

    public override NodeState Evaluate()
    {
        _context.PossibleDisplayChoices = new List<DisplaySlotController>(DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems());
        state =NodeState.SUCCESS;
        return state;
    }
}
