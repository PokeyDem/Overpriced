

public class StatePutItemOnDisplay : BaseTutorialState
{
    public StatePutItemOnDisplay(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.GoOutsideTrigger.isActive = false;
        _tutorialManager.OpenShopButton.interactable = false;
        _tutorialManager.PutItemOnDisplayText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.GoOutsideTrigger.isActive = true;
        _tutorialManager.OpenShopButton.interactable = true;
        _tutorialManager.PutItemOnDisplayText.SetActive(false);
    }
}
