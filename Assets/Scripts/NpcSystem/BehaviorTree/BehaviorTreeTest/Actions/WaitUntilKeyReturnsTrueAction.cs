using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeTest;

public class WaitUntilKeyReturnsTrueAction : BTNode
{
    private string _key;

    public WaitUntilKeyReturnsTrueAction(string key)
    {
        _key = key;
    }

    protected override NodeState OnUpdate()
    {
        bool? value = Blackboard.Get<bool?>(_key, null);
        if (value == null)
        {
            Debug.Log($"{_key} is null, waiting for True");
            return NodeState.Running;
        }
        if (value == true)
        {
            Debug.Log($"{_key} is true");
            return NodeState.Success;
        }
        else
        {
            Debug.Log($"{_key} is false, waiting for True");
            return NodeState.Running;
        }
    }

}
