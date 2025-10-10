using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class NpcManager : MonoBehaviour
{
    [SerializeField] List<NpcBehaviour> _npcPrefabs;
    [SerializeField] private List<NPCDesiredItems> _npcDesiredItems;
    [SerializeField] private int _number;
    [SerializeField] ItemsDatabaseSO _itemsDatabase;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _despawnPointPos;
    [SerializeField] Transform _windowPos;
    [SerializeField] Transform _doorPos;
    [SerializeField] private Transform _despawnInShop;
    [SerializeField] private List<Transform> _counterPos;
    public static List<bool> counterTaken = new List<bool>();  
    [SerializeField] private float _minDelay;//Todo move to config;
    [SerializeField] private float _maxDelay;//Todo move to config
    [SerializeField] private bool debugSpawn;
    [SerializeField] private List<DailyNPCSpawns> _dailyNpcSpawns;
    private int _npcsCount;
    private bool _spawnCoroutineChecker;

    private List<IObjectPool<NpcBehaviour>> _objectPools;
    [SerializeField] private int _DefaultCapacity = 20;
    [SerializeField] private int _MaxSize = 100;

    private void Awake()
    {
        if (_counterPos.Count!=0)
        {
            for(int i=0; i< _counterPos.Count; i++)
            {
                counterTaken.Add(false);
            }
        }
        _objectPools=new List<IObjectPool<NpcBehaviour>>();
        foreach(NpcBehaviour prefab in _npcPrefabs)
        {
            _objectPools.Add(new ObjectPool<NpcBehaviour>(() => CreateNpc(prefab), OnGetFromPool, OnReleaseToPool,
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
        NpcBehaviour npcPrefab = _npcPrefabs[3];

        var npc=_objectPools[(int)npcPrefab.GetNpcType()].Get();
        var desiredItems = _npcDesiredItems[(int)npcPrefab.GetNpcType()].GetDesiredItems();
        npc.transform.position = _spawnPoint.position;
        npc.Initialize(false, _spawnPoint.position, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, null, _counterPos, desiredItems, _objectPools[3] );
        _npcsCount++;
    }
    private void SpawnNpc(NpcBehaviour npcPrefab)
    {
        var npc = _objectPools[(int)npcPrefab.GetNpcType()].Get();
        var desiredItems = _npcDesiredItems[(int)npcPrefab.GetNpcType()].GetDesiredItems();
        npc.Initialize(false, _spawnPoint.position, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, null, _counterPos, desiredItems, _objectPools[(int)npc.GetNpcType()]);
        _npcsCount++;
    }

    public void DespawnNpc(NpcBehaviour npc){
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
    public NpcBehaviour CreateNpc(NpcBehaviour prefab)
    {
        NpcBehaviour npcBehaviour = Instantiate(prefab,_spawnPoint.position,Quaternion.identity);

        return npcBehaviour;
    }
    private void OnReleaseToPool(NpcBehaviour pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnGetFromPool(NpcBehaviour pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnDestroyPooledObject(NpcBehaviour pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
