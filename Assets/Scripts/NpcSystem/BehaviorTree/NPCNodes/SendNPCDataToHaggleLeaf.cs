using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SendNPCDataToHaggleLeaf : Node
{
    IHaggler _haggler;
    IHasDisplayTarget _hasDisplayTarget;
    float _toleranceDecimal;
    NPCType _npcType;
    IMoodController _moodController;
    INPCReadyToHaggleController _readyToHaggleController;

    public SendNPCDataToHaggleLeaf(IHaggler haggler, IHasDisplayTarget hasDisplayTarget, float toleranceDecimal, NPCType npcType, IMoodController moodController, INPCReadyToHaggleController readyToHaggleController)
    {
        _haggler = haggler;
        _hasDisplayTarget = hasDisplayTarget;
        _toleranceDecimal = toleranceDecimal;
        _npcType = npcType;
        _moodController = moodController;
        _readyToHaggleController = readyToHaggleController;
    }

    public override NodeState Evaluate()
    {
        if (_readyToHaggleController == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _readyToHaggleController.NotifyNPCReadyToHaggle(_haggler, _hasDisplayTarget, _toleranceDecimal, _npcType, _moodController);
        state = NodeState.SUCCESS;
        return state;
    }
}
