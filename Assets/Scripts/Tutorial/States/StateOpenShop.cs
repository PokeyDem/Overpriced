

public class StateOpenShop : BaseTutorialState
{
    public StateOpenShop(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.GoOutsideTrigger.isActive = false;
        _tutorialManager.OpenShopText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.GoOutsideTrigger.isActive = true;
        _tutorialManager.OpenShopText.SetActive(false);
    }
}
