

public class State05_PutItemOnDisplay : BaseTutorialState
{
    public State05_PutItemOnDisplay(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.MerchantGuildTrigger.isActive = false;
        _tutorialManager.OpenShopButton.interactable = false;
        _tutorialManager.PutItemOnDisplayText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.MerchantGuildTrigger.isActive = true;
        _tutorialManager.OpenShopButton.interactable = true;
        _tutorialManager.PutItemOnDisplayText.SetActive(false);
    }
}
