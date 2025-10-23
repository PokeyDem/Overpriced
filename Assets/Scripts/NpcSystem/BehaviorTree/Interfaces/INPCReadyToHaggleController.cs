using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCReadyToHaggleController
{
    public void NotifyNPCReadyToHaggle(IHaggler haggler, IHasDisplayTarget displayTargetActor, float toleranceDecimal, NPCType npcType, IMoodController moodController);
}
