

public abstract class BaseTutorialState 
{
    protected TutorialManager _tutorialManager;
    public BaseTutorialState(TutorialManager tutorialManager)
    {
        _tutorialManager = tutorialManager;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}