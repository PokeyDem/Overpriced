namespace BehaviorTreeTest
{
    public class DespawnAction : BTNode
    {
        private IDespawnable _despawnable;
        public DespawnAction(IDespawnable despawnable)
        {
            _despawnable = despawnable;
        }
        protected override NodeState OnUpdate()
        {
            _despawnable.Despawn();
            return NodeState.Success;
        }

    }
}

