

public class State03_BuyItems : BaseTutorialState
{
    public State03_BuyItems(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.MerchantGuildExitButton.interactable = false;
        _tutorialManager.ChooseitemToBuyText.SetActive(true);
        _tutorialManager.BuyItemsText.SetActive(true);
    }
    public override void Exit()
    {
        _tutorialManager.MerchantGuildExitButton.interactable = true;
        _tutorialManager.ChooseitemToBuyText.SetActive(false);
        _tutorialManager.BuyItemsText.SetActive(false);
    }
}
