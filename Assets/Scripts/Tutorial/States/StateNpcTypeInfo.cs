using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNpcTypeInfo : BaseTutorialState
{
    public StateNpcTypeInfo(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.NpcTypeInfoText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.NpcTypeInfoText.SetActive(false);
    }
}
