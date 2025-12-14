using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBuyDisplays : BaseTutorialState
{
    public StateBuyDisplays(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.BuyDisplayText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.BuyDisplayText.SetActive(false);
    }
}
