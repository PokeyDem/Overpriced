using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace GOAP 
{ 
    public class MoveStrategy : IActionStrategy
    {
        readonly NavMeshAgent _agent;
        readonly Func<Vector3> _destination;

        public bool CanPerform => !Complete;

        public bool Complete => _agent.remainingDistance<= _agent.stoppingDistance && !_agent.pathPending;

        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination)
        {
            _agent = agent;
            _destination = destination;
        }
        public void Start() => _agent.SetDestination(_destination());
        public void Stop() => _agent.ResetPath();
    }
}
