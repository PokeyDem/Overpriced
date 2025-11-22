using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeTest;

public class SetKeyAction<T> : BTNode
{
    private string _key;
    private object _value;

    public SetKeyAction(string key, T value)
    {
        _key = key;
        _value = value;
    }

    protected override NodeState OnUpdate()
    {
        Blackboard.Set(_key, _value);
        Debug.Log($"set {_key} to {_value}");
        return NodeState.Success;
    }

}
