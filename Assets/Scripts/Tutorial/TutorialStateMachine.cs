using System;

public class TutorialStateMachine 
{
    public BaseTutorialState CurrentState {  get; private set; }
    public State01_Start state01_Start;
    public State02_GoToShop state02_GoToShop;
    public State03_BuyItems state03_BuyItems;
    public State04_ExitMerchantGuild state04_ExitMerchantGuild;
    public State05_PutItemOnDisplay state05_PutItemOnDisplay;

    public event Action<BaseTutorialState> StateChanged;
    public TutorialStateMachine(TutorialManager manager)
    {
        state01_Start = new State01_Start(manager);
        state02_GoToShop = new State02_GoToShop(manager);
        state03_BuyItems = new State03_BuyItems(manager);
        state04_ExitMerchantGuild = new State04_ExitMerchantGuild(manager);
        state05_PutItemOnDisplay = new State05_PutItemOnDisplay(manager);
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
