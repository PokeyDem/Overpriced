using System;

public class NPCStateMachine
{
    public NpcBehaviour NpcBehaviour { get; private set; }
    public BaseState CurrentState { get; private set; }

    public event Action<BaseState> StateChanged;

    public NPCStateMachine(NpcBehaviour npcBehaviour)
    {
        this.NpcBehaviour = npcBehaviour;
    }

    public void Initialize(BaseState state)
    {
        CurrentState = state;
        CurrentState.SetMachine(this);
        state.Enter();
        NpcBehaviour.Debg($"{this.GetHashCode()} entered {state.GetType().Name}");

        StateChanged?.Invoke(state);
    }

    public void TransitionTo(BaseState nextState)
    {
        CurrentState.Exit();
        CurrentState= nextState;
        CurrentState.SetMachine(this);
        nextState.Enter();
        NpcBehaviour.Debg($"{this.GetHashCode()} entered {nextState.GetType().Name}");

        StateChanged?.Invoke(nextState);
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

}
