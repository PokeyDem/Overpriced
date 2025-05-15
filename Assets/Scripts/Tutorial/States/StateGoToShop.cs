using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGoToShop : BaseTutorialState
{
    public StateGoToShop(TutorialManager tutorialManager) : base(tutorialManager)
    {
        _type = TutorialStateType.StateGoToShop;
    }
    public override void Enter()
    {
        //_tutorialManager.PlayerDisplayInteraction.enabled = false;
        _tutorialManager.GoToShopText.SetActive(true);
    }
    public override void Exit()
    {
        //_tutorialManager.PlayerDisplayInteraction.enabled = true;
        _tutorialManager.GoToShopText.SetActive(false);
    }
}
