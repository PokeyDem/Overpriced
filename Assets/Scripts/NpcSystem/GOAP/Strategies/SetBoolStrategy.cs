using System;
using UnityEngine.AI;

namespace GOAP 
{
    public class SetBoolStrategy : IActionStrategy
    {
        public bool CanPerform => true;

        public bool Complete { get; private set; }
        readonly Action<bool> _flag;
        Func<bool> _condition;

        public SetBoolStrategy(Action<bool> flag, Func<bool> condition)
        {
            _flag = flag;
            _condition = condition;
        }
        public void Start()
        {
            _flag(_condition());
            Complete = true;
        }
    }
}

