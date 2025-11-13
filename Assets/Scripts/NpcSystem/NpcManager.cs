using DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class NpcManager : SingletonWithDestroy<NpcManager>, ILinePositionManager, IDependencyProvider, INPCReadyToHaggleController
{

    #region Serialized Fields
    [SerializeField] private List<NPCBehaviorTree> _npcPrefabs;
    [SerializeField] private List<NPCDesiredItems> _npcDesiredItems;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _despawnPointPos;
    [SerializeField] private Transform _windowPos;
    [SerializeField] private Transform _doorPos;
    [SerializeField] private Transform _shopSpawnPoint;
    [SerializeField] private Transform _despawnInShop;
    [SerializeField] private List<Transform> _counterPos;
    [SerializeField] private List<DailyNPCSpawns> _dailyNpcSpawns;
    #endregion

    private int _npcsCount;

    [SerializeField]
    private List<bool> _positionsInLineOccupancy = new List<bool>();

    public static event Action<IHaggler, IHasDisplayTarget, float, NPCType, IMoodController> OnNpcReadyToHaggle;

    #region Object Pooling Variables
    private List<IObjectPool<NPCBehaviorTree>> _objectPools;
    [SerializeField] private int _DefaultCapacity = 20;
    [SerializeField] private int _MaxSize = 100;
    #endregion

    private new void Awake()
    {
        base.Awake();
        if (_counterPos.Count != 0)
        {
            for (int i = 0; i < _counterPos.Count; i++)
            {
                _positionsInLineOccupancy.Add(false);
            }
        }
        _objectPools = new List<IObjectPool<NPCBehaviorTree>>();
        foreach (NPCBehaviorTree prefab in _npcPrefabs)
        {
            _objectPools.Add(new ObjectPool<NPCBehaviorTree>(() => CreateNpc(prefab), OnGetFromPool, OnReleaseToPool,
                            OnDestroyPooledObject, true, _DefaultCapacity, _MaxSize));
        }
    }
    #region Methods

    public void InitializeNPCScenario()//used in ShopStateManager event
    {
        int day = DayManager.Instance.GetDay();
        DayManager.PartOfDay timeOfDay = DayManager.Instance.GetPartOfDay();
        StartCoroutine(SpawnNPCScenario(day, timeOfDay));
    }
    private void SpawnNpc(NPCBehaviorTree npcPrefab)
    {
        var npc = _objectPools[(int)npcPrefab.GetNpcType()].Get();
        var desiredItems = _npcDesiredItems[(int)npcPrefab.GetNpcType()].GetDesiredItems();
        npc.Initialize(false, _spawnPoint.position, _despawnPointPos.position, _despawnInShop.position, _windowPos.position, _doorPos.position, _shopSpawnPoint.position, _counterPos.Select(t => t.position).ToList(), desiredItems, this, this, _objectPools[(int)npc.GetNpcType()]);
        _npcsCount++;
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
            for (int i = 0; i < npcGroupSpawn.SpawnCount; i++)
            {
                SpawnNpc(_npcPrefabs[(int)npcGroupSpawn.NpcType]);
                float random = Random.Range(400, 800);
                random /= 100;
                yield return new WaitForSeconds(random);
            }
        }
        yield return new WaitUntil(() => _npcsCount == 0);
        ShopStateManager.ShopStateManagerInstance.CloseShop();
    }
    public bool GetPositionInLineOccupancy(int i)
    {
        return _positionsInLineOccupancy[i];
    }

    public void SetPositionInLineOccupancy(int i, bool isOccupied)
    {
        _positionsInLineOccupancy[i] = isOccupied;
    }
    #endregion
    #region Object Pooling Methods
    public NPCBehaviorTree CreateNpc(NPCBehaviorTree prefab)
    {
        NPCBehaviorTree npcBehaviour = Instantiate(prefab, _spawnPoint.position, Quaternion.identity);

        return npcBehaviour;
    }
    public void DespawnNpc(NPCBehaviorTree npc)
    {
        npc.ReturnToPool();
        _npcsCount--;
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

    public void NotifyNPCReadyToHaggle(IHaggler haggler, IHasDisplayTarget displayTargetActor, float toleranceDecimal, NPCType npcType, IMoodController moodController)
    {
        OnNpcReadyToHaggle?.Invoke(haggler, displayTargetActor, toleranceDecimal, npcType, moodController);
    }
    #endregion

}
