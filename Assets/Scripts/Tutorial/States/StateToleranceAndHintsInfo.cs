using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToleranceAndHintsInfo : BaseTutorialState
{
    public StateToleranceAndHintsInfo(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.ToleranceText1.SetActive(true);
        _tutorialManager.ToleranceText2.SetActive(true);
        _tutorialManager.ChancesLeftText.SetActive(true);
        _tutorialManager.HintText1.SetActive(true);
        _tutorialManager.HintText2.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.ToleranceText1.SetActive(false);
        _tutorialManager.ToleranceText2.SetActive(false);
        _tutorialManager.ChancesLeftText.SetActive(false);
        _tutorialManager.HintText1.SetActive(false);
        _tutorialManager.HintText2.SetActive(false);
    }
}
