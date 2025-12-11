using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTrySell : BaseTutorialState
{
    public StateTrySell(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.AmountSlider.enabled = false;
        _tutorialManager.InputField.enabled = false;
        _tutorialManager.TryToSellText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.AmountSlider.enabled = true;
        _tutorialManager.InputField.enabled = true;
        _tutorialManager.TryToSellText.SetActive(false);
    }
}
