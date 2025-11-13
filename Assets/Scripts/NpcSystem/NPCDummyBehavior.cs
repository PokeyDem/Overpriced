using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class NPCDummyBehavior : MonoBehaviour
{
    private IObjectPool<NPCDummyBehavior> _pool;
    private NavMeshAgent _agent;
    [SerializeField] private NPCType _NPCType;

    public void Initialize(Vector3 spawnPointPos, Vector3 despawnPointPos, float speed, IObjectPool<NPCDummyBehavior> pool)
    {
        _pool = pool;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed / 100;
        _agent.Warp(spawnPointPos);
        _agent.transform.LookAt(despawnPointPos);
        _agent.SetDestination(despawnPointPos);
        StartCoroutine(WaitUntilArrived());
    }

    public void Despawn()
    {
        NPCDummySpawner.Instance.DespawnNpc(this);
    }
    public void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitUntilArrived()
    {
        yield return new WaitUntil(() => !_agent.pathPending);

        while (_agent.remainingDistance > _agent.stoppingDistance)
        {
            yield return null;
        }

        Despawn();
    }
    public NPCType GetNpcType()
    {
        return _NPCType;
    }
}
