

using UnityEngine;

public class DecideShopEntryState : BaseState
{
    public override void Enter()
    {
        Vector3 targetPos;
        bool hasItems = DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count != 0;
        if (hasItems)
            targetPos = _machine.NpcBehaviour._doorPos.position;
        else targetPos = _machine.NpcBehaviour._despawnPointPos.position;
        _machine.NpcBehaviour._agent.SetDestination(targetPos);
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (!_machine.NpcBehaviour._agent.pathPending && _machine.NpcBehaviour._agent.remainingDistance <= _machine.NpcBehaviour._agent.stoppingDistance)
        {
            _machine.TransitionTo(new NPCWaitState(2, 6, new IdleState()));
        }
    }
}
