using System;

public class TutorialStateMachine 
{
    public BaseTutorialState CurrentState {  get; private set; }
    public StateNothing stateNothing;
    public StateGoToShop stateGoToShop;
    public StateEnterMerchantGuild stateEnterMerchantGuild;
    public StateBuyItems stateBuyItems;
    public StateSpendAllMoney stateSpendAllMoney;
    public StateExitMerchantGuild stateExitMerchantGuild;
    public StateEnterShop stateEnterShop;
    public StatePutItemOnDisplay statePutItemOnDisplay;
    public StateOpenShop stateOpenShop;
    public StateWaitForBuyer stateWaitForBuyer;
    public StateStartHaggling stateStartHaggling;
    public StateChangePriceValue stateChangePriceValue;
    public StateChangePriceValue2 stateChangePriceValue2;
    public StateTrySell stateTrySell;
    public StateEnd stateEnd;

    public event Action<BaseTutorialState> StateChanged;
    public TutorialStateMachine(TutorialManager manager)
    {
        stateNothing = new StateNothing(manager);
        stateGoToShop = new StateGoToShop(manager);
        stateEnterMerchantGuild= new StateEnterMerchantGuild(manager);
        stateBuyItems = new StateBuyItems(manager);
        stateSpendAllMoney = new StateSpendAllMoney(manager);
        stateExitMerchantGuild = new StateExitMerchantGuild(manager);
        stateEnterShop = new StateEnterShop(manager);
        statePutItemOnDisplay = new StatePutItemOnDisplay(manager);
        stateOpenShop = new StateOpenShop(manager);
        stateWaitForBuyer = new StateWaitForBuyer(manager);
        stateStartHaggling = new StateStartHaggling(manager);
        stateChangePriceValue = new StateChangePriceValue(manager);
        stateTrySell = new StateTrySell(manager);
        stateEnd = new StateEnd(manager);
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
