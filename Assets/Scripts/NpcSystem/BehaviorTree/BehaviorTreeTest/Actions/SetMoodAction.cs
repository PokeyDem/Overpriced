using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTreeTest
{
    public class SetMoodAction : BTNode
    {
        private IMoodController _moodController;
        private MoodType _moodType;

        public SetMoodAction(IMoodController moodController, MoodType moodType)
        {
            _moodController = moodController;
            _moodType = moodType;
        }

        protected override NodeState OnUpdate()
        {
            _moodController.InvokeMoodChange(_moodType);
            return NodeState.Success;
        }
    }

}
