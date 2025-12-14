using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTalentsInfo : BaseTutorialState
{
    public StateTalentsInfo(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.GoOutsideTrigger.isActive = false;
        _tutorialManager.OpenShopButton.interactable = false;
        _tutorialManager.TalentText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.GoOutsideTrigger.isActive = true;
        _tutorialManager.OpenShopButton.interactable = true;
        _tutorialManager.TalentText.SetActive(false);
    }
}
