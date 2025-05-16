
public class StateEnd : BaseTutorialState
{
    public StateEnd(TutorialManager tutorialManager) : base(tutorialManager)
    {
    }
    public override void Enter()
    {
        _tutorialManager.DisableTutorial();
    }
    public override void Exit()
    {
    }
}
