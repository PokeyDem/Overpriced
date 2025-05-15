using System;

public class TutorialStateMachine 
{
    public BaseTutorialState CurrentState {  get; private set; }
    public StateNothing stateNothing;
    public StateGoToShop stateGoToShop;
    public StateBuyItems stateBuyItems;
    public StateExitMerchantGuild stateExitMerchantGuild;
    public StatePutItemOnDisplay statePutItemOnDisplay;
    public StateOpenShop stateOpenShop;
    public StateWaitForBuyer stateWaitForBuyer;
    public StateStartHaggling stateStartHaggling;
    public StateChangePriceValue stateChangePriceValue;
    public StateTrySell stateTrySell;

    public event Action<BaseTutorialState> StateChanged;
    public TutorialStateMachine(TutorialManager manager)
    {
        stateNothing = new StateNothing(manager);
        stateGoToShop = new StateGoToShop(manager);
        stateBuyItems = new StateBuyItems(manager);
        stateExitMerchantGuild = new StateExitMerchantGuild(manager);
        statePutItemOnDisplay = new StatePutItemOnDisplay(manager);
        stateOpenShop = new StateOpenShop(manager);
        stateWaitForBuyer = new StateWaitForBuyer(manager);
        stateStartHaggling = new StateStartHaggling(manager);
        stateChangePriceValue = new StateChangePriceValue(manager);
        stateTrySell = new StateTrySell(manager);
    }
    public void Initialize(BaseTutorialState state)
    {
        CurrentState = state;
        state.Enter();

        StateChanged?.Invoke(state);
    }
    public void TransitionTo(BaseTutorialState state)
    {
        CurrentState.Exit();
        CurrentState = state;
        state.Enter();
        StateChanged?.Invoke(state);
    }
    public void Update()
    {
        if (CurrentState!=null) {
            CurrentState.Update();
        }
    }
}
