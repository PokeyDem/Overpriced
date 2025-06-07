using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnterMerchantGuild : BaseTutorialState
{
    public StateEnterMerchantGuild(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.GoBackToShopButton.interactable = false;
        _tutorialManager.GoToGuildText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.GoBackToShopButton.interactable = true;
        _tutorialManager.GoToGuildText.SetActive(false);
    }
}
