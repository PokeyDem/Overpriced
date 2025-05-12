
using UnityEngine.Events;

public class TutorialNextStepCommand : ICommand
{
    private UnityEvent _previousEvent;
    private TutorialNextStepCommand _previousCommand;

    private UnityEvent _nextEvent;
    private TutorialNextStepCommand _nextCommand;

    private BaseTutorialState _state;
    private TutorialStateMachine _stateMachine;


    public UnityEvent NextEvent => _nextEvent;
    public UnityEvent PreviousEvent
    {
        get { return _previousEvent; }
        set { _previousEvent = value; }
    }
    public TutorialNextStepCommand NextCommand { 
        get { return _nextCommand;} 
        set {
            _nextCommand = value;
        }
    }
    public TutorialNextStepCommand(TutorialNextStepCommand previousCommand,UnityEvent nextEvent, BaseTutorialState state, TutorialStateMachine stateMachine) 
    {
        this._previousCommand = previousCommand;
        this._nextEvent = nextEvent;
        this._state = state;
        this._stateMachine = stateMachine;
        if(previousCommand != null)
        {
            this._previousEvent = previousCommand.NextEvent;
            previousCommand.NextCommand = this;
        }
    }
    public void Execute()
    {
        if (_previousEvent != null)
        {
            _previousEvent.RemoveListener(this.Execute);
        }
        _stateMachine.TransitionTo(_state);
        if(_nextEvent != null&&_nextCommand!=null)
        {
            _nextEvent.AddListener(_nextCommand.Execute);
        }
    }
}
