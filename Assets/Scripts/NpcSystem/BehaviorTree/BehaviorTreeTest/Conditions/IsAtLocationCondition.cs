using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

namespace BehaviorTreeTest
{
    public class IsAtLocationCondition : BTNode
    {
        private NavMeshAgent _agent;
        private Vector3 _location;
        private float _range;

        public IsAtLocationCondition(NavMeshAgent agent, Vector3 location, float range)
        {
            this._agent = agent;
            this._location = location;
            this._range = range;
        }

        protected override NodeState OnUpdate()
        {
            Vector2 agentPos=new Vector2(_agent.transform.position.x, _agent.transform.position.z);
            Vector2 location = new Vector2(_location.x, _location.z);
            bool inRange = Vector2.Distance(agentPos, location) < _range;
            if (inRange)
            {
                Debug.Log($"{agentPos} is at location");
                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }
        }
    }
}