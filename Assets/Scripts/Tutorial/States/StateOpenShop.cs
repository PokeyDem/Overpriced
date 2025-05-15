

public class StateOpenShop : BaseTutorialState
{
    public StateOpenShop(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.MerchantGuildTrigger.isActive = false;
        _tutorialManager.OpenShopText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.MerchantGuildTrigger.isActive = true;
        _tutorialManager.OpenShopText.SetActive(false);
    }
}
