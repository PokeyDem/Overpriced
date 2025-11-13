using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NPCDummySpawner : SingletonWithDestroy<NPCDummySpawner>
{
    public bool isPaused = false;
    [SerializeField] private float _interval = 2f;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _despawnPointPos;
    [SerializeField] private List<NPCDummyBehavior> _npcPrefabs;
    private float timer = 0f;

    #region Object Pooling Variables
    private List<IObjectPool<NPCDummyBehavior>> _objectPools;
    [SerializeField] private int _DefaultCapacity = 20;
    [SerializeField] private int _MaxSize = 100;
    #endregion

    private new void Awake()
    {
        base.Awake();
        _objectPools = new List<IObjectPool<NPCDummyBehavior>>();
        foreach (NPCDummyBehavior prefab in _npcPrefabs)
        {
            _objectPools.Add(new ObjectPool<NPCDummyBehavior>(() => CreateNpc(prefab), OnGetFromPool, OnReleaseToPool,
                            OnDestroyPooledObject, true, _DefaultCapacity, _MaxSize));
        }
    }

    void Update()
    {
        if (isPaused) return;
        timer += Time.deltaTime;
        if (timer >= _interval)
        {
            NPCDummyBehavior npcPrefab = _npcPrefabs[0];
            switch (Random.Range(0,100))
            {
                case >= 95:
                    npcPrefab = _npcPrefabs[3];
                    break;
                case >= 80:
                    npcPrefab = _npcPrefabs[2];
                    break;
                case >= 55:
                    npcPrefab = _npcPrefabs[1];
                    break;
                default:
                    npcPrefab = _npcPrefabs[0];
                    break;
            }
            StartCoroutine(SpawnMultiple(npcPrefab));
            _interval = Random.Range(300, 400) / 100;
            timer = 0f;
        }
    }

    private void SpawnNpc(NPCDummyBehavior npcPrefab, Vector3 spawnpoint, Vector3 despawnpoint, float speed)
    {
        var npc = _objectPools[(int)npcPrefab.GetNpcType()].Get();

        npc.Initialize(spawnpoint, despawnpoint, speed, _objectPools[(int)npc.GetNpcType()]);
    }

    public NPCDummyBehavior CreateNpc(NPCDummyBehavior prefab)
    {
        NPCDummyBehavior npcBehaviour = Instantiate(prefab, _spawnPoint.position, Quaternion.identity);

        return npcBehaviour;
    }
    public void DespawnNpc(NPCDummyBehavior npc)
    {
        npc.ReturnToPool();
    }
    private void OnReleaseToPool(NPCDummyBehavior pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnGetFromPool(NPCDummyBehavior pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnDestroyPooledObject(NPCDummyBehavior pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
    private Vector3 GeneratePositionDeviation(float minZ, float maxZ)
    {
        float randomZ = UnityEngine.Random.Range(minZ, maxZ) / 100;
        return new Vector3(0, 0, randomZ);
    }

    private IEnumerator SpawnMultiple(NPCDummyBehavior npcPrefab)
    {
        int rand = Random.Range(0, 100);

        int result;
        if (rand < 75)      
            result = 1;
        else if (rand < 95) 
            result = 2;
        else                 
            result = 3;
        if (npcPrefab.GetNpcType() == NPCType.GuildMaster) result = 1;
        Vector3 spawnpoint = new Vector3(0, 0, 0);
        Vector3 despawnpoint = new Vector3(0, 0, 0);
        float deviationZ = 0;
        switch (Random.Range(0, 100))
        {
            case >= 50:
                deviationZ = -0.4f;
                spawnpoint = _spawnPoint.position + new Vector3(0,0,0.6f);
                despawnpoint = _despawnPointPos.position + new Vector3(0, 0, 0.6f);
                break;
            default:
                deviationZ = 0.4f;
                despawnpoint = _spawnPoint.position + new Vector3(0, 0, -0.1f);
                spawnpoint = _despawnPointPos.position + new Vector3(0, 0, -0.1f);
                break;
        }
        float speed = Random.Range(80, 150);
        Vector3 deviation = new Vector3(0, 0, 0);
        for (int i = 1; i <= result; i++)
        {
            SpawnNpc(npcPrefab,spawnpoint+deviation,despawnpoint+deviation, speed);
            int value = (i % 2 == 0) ? 1 : -1;
            deviation = new Vector3(0, 0, deviationZ * (value));
            yield return new WaitForSeconds(0.3f);
        }
    }
}
