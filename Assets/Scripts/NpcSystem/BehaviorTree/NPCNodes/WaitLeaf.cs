using BehaviorTree;
using UnityEngine;

public class WaitLeaf : Node
{
    private bool _started=false;
    private float _duration;
    private float _startTime;
    private IMoodController _moodController;
    public WaitLeaf(float duration, IMoodController moodController)
    {
        _moodController = moodController;
        _duration =duration;
    }
    public WaitLeaf(float duration)
    {
        _moodController = null;
        _duration = duration;
    }
    public override NodeState Evaluate()
    {
        if (!_started)
        {
            _started = true;
            _startTime = Time.time;
            if (_moodController != null) 
            {
                _moodController.InvokeMoodChange(MoodType.Thinking);
            }
            state=NodeState.RUNNING;
            return state;
        }
        if (Time.time - _startTime >= _duration)
        {
            _started = false;
            if (_moodController != null)
            {
                _moodController.InvokeMoodChange(MoodType.None);
            }
            state = NodeState.SUCCESS;
            return state;
        }
        state=NodeState.RUNNING;
        return state;
    }
}
