using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class NpcManager : MonoBehaviour{
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
    private GameObject[] _npcs;
    private int _npcsCount;
    private bool _spawnCoroutineChecker;
    
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space) && debugSpawn){
            SpawnNpc();
        }
    }

    private void SpawnNpc(){
        if (ShopStateManager.ShopStateManagerInstance.ShopIsClose() || _npcsCount>0) {
            return;
        }
        //ItemData item = _itemsDatabase._itemsData[Random.Range(0,_itemsDatabase._itemsData.Count)]; //Todo make it random when there is more items (moved to Npc{npctype}.cs)
        GameObject npcPrefab = _npcPrefabs[Random.Range(0, _npcPrefabs.Count)];
        var npc = Instantiate(npcPrefab, _spawnPoint.position, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(null, false, _despawnPointPos, _despawnInShop,_windowPos, _doorPos, null, _counterPos);
        _npcsCount++;
    }

    public void SpawnNpcInsideShop(Vector3 spawnPoint, ItemData item, GameObject display, NPCType npcType){
        GameObject npcPrefab = _npcPrefabs.Find(x => x.GetComponent<NpcBehaviour>().GetNpcType() == npcType);
        var npc = Instantiate(npcPrefab, spawnPoint, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(item, true, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, display, _counterPos);
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
}
