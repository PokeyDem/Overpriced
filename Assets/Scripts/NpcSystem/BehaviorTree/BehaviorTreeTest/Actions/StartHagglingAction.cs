
using UnityEngine;

namespace BehaviorTreeTest
{
    public class StartHagglingAction : BTNode
    {
        IHaggler _haggler;
        IHasDisplayTarget _hasDisplayTarget;
        float _toleranceDecimal;
        NPCType _npcType;
        IMoodController _moodController;
        INPCReadyToHaggleController _readyToHaggleController;

        public StartHagglingAction(IHaggler haggler, IHasDisplayTarget hasDisplayTarget, float toleranceDecimal, NPCType npcType, IMoodController moodController, INPCReadyToHaggleController readyToHaggleController)
        {
            _haggler = haggler;
            _hasDisplayTarget = hasDisplayTarget;
            _toleranceDecimal = toleranceDecimal;
            _npcType = npcType;
            _moodController = moodController;
            _readyToHaggleController = readyToHaggleController;
        }
        protected override NodeState OnUpdate()
        {
            if (_readyToHaggleController == null)
            {
                Debug.Log("ready to haggle controller is null");
                return NodeState.Failure;
            }

            _readyToHaggleController.NotifyNPCReadyToHaggle(_haggler, _hasDisplayTarget, _toleranceDecimal, _npcType, _moodController);
            return NodeState.Success;
        }
    }
}
