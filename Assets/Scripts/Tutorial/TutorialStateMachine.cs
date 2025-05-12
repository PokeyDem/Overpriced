using System;

public class TutorialStateMachine 
{
    public BaseTutorialState CurrentState {  get; private set; }
    public State01_Start state01_Start;
    public State02_GoToShop state02_GoToShop;
    public State03_BuyItems state03_BuyItems;
    public State04_ExitMerchantGuild state04_ExitMerchantGuild;
    public State05_PutItemOnDisplay state05_PutItemOnDisplay;
    public State06_OpenShop state06_OpenShop;
    public State07_WaitForBuyer state07_WaitForBuyer;
    public State08_StartHaggling state08_StartHaggling;

    public event Action<BaseTutorialState> StateChanged;
    public TutorialStateMachine(TutorialManager manager)
    {
        state01_Start = new State01_Start(manager);
        state02_GoToShop = new State02_GoToShop(manager);
        state03_BuyItems = new State03_BuyItems(manager);
        state04_ExitMerchantGuild = new State04_ExitMerchantGuild(manager);
        state05_PutItemOnDisplay = new State05_PutItemOnDisplay(manager);
        state06_OpenShop = new State06_OpenShop(manager);
        state07_WaitForBuyer = new State07_WaitForBuyer(manager);
        state08_StartHaggling = new State08_StartHaggling(manager);
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
