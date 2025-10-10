

public abstract class BaseState 
{
    protected NPCStateMachine _machine;

    public void SetMachine(NPCStateMachine machine)
    {
        _machine = machine;
    }
    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();

}
