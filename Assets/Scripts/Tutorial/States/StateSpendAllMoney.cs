using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSpendAllMoney : BaseTutorialState
{
    public StateSpendAllMoney(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.MerchantGuildExitButton.interactable = false;
        _tutorialManager.SpendMoneyText.SetActive(true);
        _tutorialManager.SpendMoneyText2.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.MerchantGuildExitButton.interactable = true;
        _tutorialManager.SpendMoneyText.SetActive(false);
        _tutorialManager.SpendMoneyText2.SetActive(false);
    }
}
