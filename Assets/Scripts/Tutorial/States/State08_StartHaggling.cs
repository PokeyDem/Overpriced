using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State08_StartHaggling : BaseTutorialState
{
    public State08_StartHaggling(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.StartHagglingText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.StartHagglingText.SetActive(false);
    }
}
