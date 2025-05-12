using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State02_GoToShop : BaseTutorialState
{
    public State02_GoToShop(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.PlayerDisplayInteraction.enabled = false;
        _tutorialManager.GoToShopText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.PlayerDisplayInteraction.enabled = true;
        _tutorialManager.GoToShopText.SetActive(false);
    }
}
