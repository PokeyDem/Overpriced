using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State09_ChangePriceValue : BaseTutorialState
{
    public State09_ChangePriceValue(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.SellButton.interactable=false;
        _tutorialManager.ChangePriceValueText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.SellButton.interactable = true;
        _tutorialManager.ChangePriceValueText.SetActive(false);
    }
}
