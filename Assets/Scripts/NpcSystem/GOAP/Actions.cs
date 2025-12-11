

using System.Collections.Generic;

namespace GOAP
{
    public class AgentAction
    {
        public string Name { get; }
        public float Cost { get; private set; }

        public HashSet<AgentBelief> Preconditions { get; } = new();
        public HashSet<AgentBelief> Effects { get; } = new();

        private IActionStrategy _strategy;
        public bool Complete => _strategy.Complete;

        AgentAction(string name)
        {
            Name = name;
        }

        public void Start() => _strategy.Start();

        public void Update(float deltaTime) 
        {
            if (_strategy.CanPerform)
            {
                _strategy.Update(deltaTime); 
            }

            if (!_strategy.Complete) return;

            foreach (var effect in Effects)
            {
                effect.Evaluate();
            }
        }
        public void Stop() => _strategy.Stop();

        public class Builder
        {
            readonly AgentAction action;
            public Builder(string name)
            {
                action = new AgentAction(name)
                {
                    Cost = 1
                };
            }
            public Builder WithCost(float cost)
            {
                action.Cost = cost;
                return this;
            }
            public Builder WithStrategy(IActionStrategy strategy)
            {
                action._strategy = strategy;
                return this;
            }
            public Builder AddPrecondition(AgentBelief precondtition)
            {
                action.Preconditions.Add(precondtition);
                return this;
            }
            public Builder AddEffect(AgentBelief effect)
            {
                action.Effects.Add(effect);
                return this;
            }
            public AgentAction Build()
            {
                return action;
            }
        }
    }
}
