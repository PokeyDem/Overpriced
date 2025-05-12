
using UnityEngine.Events;

public class TutorialNextStepCommand : ICommand
{
    private UnityEvent _previousEvent;

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
            _nextCommand.PreviousEvent = _nextEvent;
        }
    }
    public TutorialNextStepCommand(UnityEvent nextEvent, BaseTutorialState state, TutorialStateMachine stateMachine) 
    {
        this._nextEvent = nextEvent;
        this._state = state;
        this._stateMachine = stateMachine;
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
