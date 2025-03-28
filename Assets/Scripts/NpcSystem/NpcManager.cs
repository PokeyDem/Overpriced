using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _npcPrefabs;
    [SerializeField] private int _number;
    [SerializeField] ItemsDatabaseSO _itemsDatabase;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _despawnPointPos;
    [SerializeField] Transform _windowPos;
    [SerializeField] Transform _doorPos;
    [SerializeField] private Transform _despawnInShop;
    [SerializeField] private Transform _counterPos;
    [SerializeField] private float _minDelay;//Todo move to config;
    [SerializeField] private float _maxDelay;//Todo move to config
    [SerializeField] private bool debugSpawn;
    [SerializeField] private List<DailyNPCSpawns> _dailyNpcSpawns;
    private int _npcsCount;
    private bool _spawnCoroutineChecker;


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
        //ItemData item = _itemsDatabase._itemsData[Random.Range(0,_itemsDatabase._itemsData.Count)]; //Todo make it random when there is more items (moved to Npc{npctype}.cs)
        //GameObject npcPrefab = _npcPrefabs[Random.Range(0, _npcPrefabs.Count)];
        GameObject npcPrefab = _npcPrefabs[3];
        var npc = Instantiate(npcPrefab, _spawnPoint.position, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(false, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, null, _counterPos);
        _npcsCount++;
    }
    private void SpawnNpc(GameObject npcPrefab)
    {
        var npc = Instantiate(npcPrefab, _spawnPoint.position, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(false, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, null, _counterPos);
        _npcsCount++;
    }

    public void SpawnNpcInsideShop(Vector3 spawnPoint, DisplaySlotController displaySlotController, NPCType npcType)
    {
        GameObject npcPrefab = _npcPrefabs.Find(x => x.GetComponent<NpcBehaviour>().GetNpcType() == npcType);
        var npc = Instantiate(npcPrefab, spawnPoint, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(true, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, displaySlotController, _counterPos);
    }

    public void DespawnNpc(GameObject npc){
        Destroy(npc);
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
        int day = DayManager.DayManagerInstance.GetDay();
        DayManager.PartOfDay timeOfDay = DayManager.DayManagerInstance.GetPartOfDay();
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
                yield return new WaitUntil(() => _npcsCount == 0);
            }
        }
        yield return new WaitForSeconds(2);
        ShopStateManager.ShopStateManagerInstance.CloseShop();
    }
}
