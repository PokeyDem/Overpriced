using System.Collections.Generic;

namespace GOAP 
{
    public class EvaluateBeliefsStrategy : IActionStrategy
    {
        private readonly IList<AgentBelief> _beliefs;
        private bool _done;

        public bool CanPerform => true;
        public bool Complete => _done;

        public EvaluateBeliefsStrategy(IList<AgentBelief> beliefs)
        {
            _beliefs = beliefs;
        }

        public void Start()
        {
            foreach (var belief in _beliefs)
            {
                bool value = belief.Evaluate();
            }

            _done = true;
        }
    }
}
