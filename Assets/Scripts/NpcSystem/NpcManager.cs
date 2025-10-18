using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class NpcManager : SingletonWithDestroy<NpcManager>
{
    [SerializeField] List<NPCBehaviorTree> _npcPrefabs;
    [SerializeField] private List<NPCDesiredItems> _npcDesiredItems;
    [SerializeField] private int _number;
    [SerializeField] ItemsDatabaseSO _itemsDatabase;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _despawnPointPos;
    [SerializeField] Transform _windowPos;
    [SerializeField] Transform _doorPos;
    [SerializeField] Transform _shopSpawnPoint;
    [SerializeField] private Transform _despawnInShop;
    [SerializeField] private List<Transform> _counterPos;
    public static List<bool> counterTaken = new List<bool>();  
    [SerializeField] private float _minDelay;//Todo move to config;
    [SerializeField] private float _maxDelay;//Todo move to config
    [SerializeField] private bool debugSpawn;
    [SerializeField] private List<DailyNPCSpawns> _dailyNpcSpawns;
    private int _npcsCount;
    private bool _spawnCoroutineChecker;

    private List<IObjectPool<NPCBehaviorTree>> _objectPools;
    [SerializeField] private int _DefaultCapacity = 20;
    [SerializeField] private int _MaxSize = 100;

    private new void Awake()
    {
        base.Awake();
        if (_counterPos.Count!=0)
        {
            for(int i=0; i< _counterPos.Count; i++)
            {
                counterTaken.Add(false);
            }
        }
        _objectPools=new List<IObjectPool<NPCBehaviorTree>>();
        foreach(NPCBehaviorTree prefab in _npcPrefabs)
        {
            _objectPools.Add(new ObjectPool<NPCBehaviorTree>(() => CreateNpc(prefab), OnGetFromPool, OnReleaseToPool,
                            OnDestroyPooledObject, true, _DefaultCapacity, _MaxSize));
        }
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space) && debugSpawn){
            SpawnNpc();
        }
    }

    private void SpawnNpc(){
        if (ShopStateManager.ShopStateManagerInstance.ShopIsClose() || _npcsCount > 0)
        {
            return;
        }
        NPCBehaviorTree npcPrefab = _npcPrefabs[3];

        var npc=_objectPools[(int)npcPrefab.GetNpcType()].Get();
        var desiredItems = _npcDesiredItems[(int)npcPrefab.GetNpcType()].GetDesiredItems();
        npc.transform.position = _spawnPoint.position;
        npc.Initialize(false, _spawnPoint.position, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, _shopSpawnPoint, null, _counterPos.Select(t => t.position).ToList(), desiredItems, _objectPools[3] );
        _npcsCount++;
    }
    private void SpawnNpc(NPCBehaviorTree npcPrefab)
    {
        var npc = _objectPools[(int)npcPrefab.GetNpcType()].Get();
        var desiredItems = _npcDesiredItems[(int)npcPrefab.GetNpcType()].GetDesiredItems();
        npc.Initialize(false, _spawnPoint.position, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, _shopSpawnPoint, null, _counterPos.Select(t => t.position).ToList(), desiredItems, _objectPools[(int)npc.GetNpcType()]);
        _npcsCount++;
    }

    public void DespawnNpc(NPCBehaviorTree npc){
        npc.ReturnToPool();
        _npcsCount--;
    }

    public void StartSpawnRandomNPC() {
        _spawnCoroutineChecker = true;
        StartCoroutine(SpawnNpcDelay());
    }
    
    public void StopSpawnRandomNPC() {
        _spawnCoroutineChecker = false;
    }

    private IEnumerator SpawnNpcDelay(){
        while (_spawnCoroutineChecker) {
            SpawnNpc();
            yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
        }
    }
    public void InitializeNPCScenario()
    {
        int day = DayManager.Instance.GetDay();
        DayManager.PartOfDay timeOfDay = DayManager.Instance.GetPartOfDay();
        StartCoroutine(SpawnNPCScenario(day, timeOfDay));
    }
    private IEnumerator SpawnNPCScenario(int day, DayManager.PartOfDay timeOfDay)
    {
        DailyNPCSpawns dailyNpcSpawn = _dailyNpcSpawns.Find(d => d.DayNr == day);

        List<NPCGroupSpawn> npcGroupSpawns = new List<NPCGroupSpawn>();
        if ((int)timeOfDay < dailyNpcSpawn.PartOfDayScenario.Count)
        {
            npcGroupSpawns = dailyNpcSpawn.PartOfDayScenario[(int)timeOfDay].scenario;
        }
        foreach (NPCGroupSpawn npcGroupSpawn in npcGroupSpawns)
        {
            for (int i = 0; i < npcGroupSpawn.SpawnCount; i++) {
                SpawnNpc(_npcPrefabs[(int)npcGroupSpawn.NpcType]);
                float random = Random.Range(50, 1000);
                random /= 100;
                yield return new WaitForSeconds(random);
            }
        }
        yield return new WaitUntil(() => _npcsCount == 0);
        ShopStateManager.ShopStateManagerInstance.CloseShop();
    }
    public NPCBehaviorTree CreateNpc(NPCBehaviorTree prefab)
    {
        NPCBehaviorTree npcBehaviour = Instantiate(prefab,_spawnPoint.position,Quaternion.identity);

        return npcBehaviour;
    }
    private void OnReleaseToPool(NPCBehaviorTree pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnGetFromPool(NPCBehaviorTree pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnDestroyPooledObject(NPCBehaviorTree pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
