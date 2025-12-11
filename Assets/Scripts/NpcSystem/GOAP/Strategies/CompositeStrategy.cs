using System.Collections.Generic;
namespace GOAP
{
    public class CompositeStrategy : IActionStrategy
    {
        private readonly Queue<IActionStrategy> _strategies;
        private IActionStrategy _current;

        public bool CanPerform => true;
        public bool Complete => _strategies.Count == 0 && (_current?.Complete ?? false);

        public CompositeStrategy(IEnumerable<IActionStrategy> strategies)
        {
            _strategies = new Queue<IActionStrategy>(strategies);
            _current = _strategies.Dequeue();
        }

        public void Start()
        {
            _current.Start();
        }

        public void Update(float dt)
        {
            if (_current == null) return;

            _current.Update(dt);

            if (_current.Complete)
            {
                _current.Stop();

                if (_strategies.Count > 0)
                {
                    _current = _strategies.Dequeue();
                    _current.Start();
                }
            }
        }

        public void Stop()
        {
            _current?.Stop();
        }
    }
}
