public enum TutorialStateType
{
    StateNothing,
    StateGoToShop,
    StateBuyItems,
    StateExitMerchantGuild,
    StatePutItemOnDisplay,
    StateOpenShop,
    StateWaitForBuyer,
    StateStartHaggling,
    StateChangePriceValue,
    StateTryToSell
}
public abstract class BaseTutorialState 
{
    protected TutorialManager _tutorialManager;
    protected TutorialStateType _type;
    public TutorialStateType Type => _type;
    public BaseTutorialState(TutorialManager tutorialManager)
    {
        _tutorialManager = tutorialManager;
        _type = TutorialStateType.StateNothing;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}