
using UnityEngine;

public class NPCWaitState : BaseState
{
    private BaseState _nextState;
    private float _timer;
    private float _waitDuration;
    private float _min, _max;

    public NPCWaitState(float min, float max, BaseState nextState)
    {
        _min = min;
        _max = max;
        _nextState = nextState;
    }
    public override void Enter()
    {
        _waitDuration= UnityEngine.Random.Range(_min, _max);
        _timer = 0f;
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _waitDuration)
        {
            _machine.TransitionTo(_nextState);
        }
    }
}
