using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCDataUser
{
    public void SetNpc(IHaggler haggler, IHasDisplayTarget displayTargetActor, float toleranceDecimal, NPCType npcType, IMoodController moodController);
}
