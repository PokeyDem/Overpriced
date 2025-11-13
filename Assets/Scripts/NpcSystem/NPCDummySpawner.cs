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
            //SpawnNpc(npcPrefab);
            StartCoroutine(SpawnMultiple(npcPrefab));
            _interval = Random.Range(300, 600) / 100;
            timer = 0f;
        }
    }

    private void SpawnNpc(NPCDummyBehavior npcPrefab)
    {
        var npc = _objectPools[(int)npcPrefab.GetNpcType()].Get();
        npc.Initialize(_spawnPoint.position+GeneratePositionDeviation(-10,60), _despawnPointPos.position, _objectPools[(int)npc.GetNpcType()]);
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
        if (rand < 60)      
            result = 1;
        else if (rand < 90) 
            result = 2;
        else                 
            result = 3;
        if (npcPrefab.GetNpcType() == NPCType.GuildMaster) result = 1;
        for (int i = 0; i < result; i++)
        {
            SpawnNpc(npcPrefab);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
