using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnterShop : BaseTutorialState
{
    public StateEnterShop(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.ReturnToShopText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.ReturnToShopText.SetActive(false);
    }
}
