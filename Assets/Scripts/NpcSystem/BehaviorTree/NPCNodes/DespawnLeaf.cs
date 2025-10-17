using BehaviorTree;
using UnityEngine;

public class DespawnLeaf : Node
{
    private IDespawnable _despawnable;
    public DespawnLeaf(IDespawnable despawnable)
    {
        _despawnable = despawnable;
    }

    public override NodeState Evaluate()
    {
        _despawnable.Despawn();
        return NodeState.SUCCESS;
    }
}
