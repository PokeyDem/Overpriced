
using UnityEngine;

public class MoveToWindowState : BaseState
{
    private Vector3 _targetPos;
    public override void Enter()
    {
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomZ = UnityEngine.Random.Range(-0.1f, 0.6f);
        Vector3 deviation = new Vector3(randomX, 0, randomZ);

        _targetPos = _machine.NpcBehaviour._windowPos.position + deviation;
        _machine.NpcBehaviour._agent.SetDestination(_targetPos);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (!_machine.NpcBehaviour._agent.pathPending && _machine.NpcBehaviour._agent.remainingDistance <= _machine.NpcBehaviour._agent.stoppingDistance)
        {
            _machine.TransitionTo(new NPCWaitState(2, 6, new DecideShopEntryState()));
        }
    }
}
