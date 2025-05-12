using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State10_TrySell : BaseTutorialState
{
    public State10_TrySell(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.TryToSellText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.TryToSellText.SetActive(false);
    }
}
