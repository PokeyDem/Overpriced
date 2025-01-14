using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcManager : MonoBehaviour{
    [SerializeField] GameObject _npcPrefab;
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
    private GameObject[] _npcs;
    private int _npcsCount;

    private void Start(){
        // SpawnNpc();  TODO !!!COMMENTED FOR DEBUG 
    }

    private void Update(){
        // if (_npcsCount == 0)
        //     StartCoroutine(SpawnNpcDelay()); TODO !!!COMMENTED FOR DEBUG 

        if (Input.GetKeyDown(KeyCode.Space)){
            SpawnNpc();
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPC")){
            Destroy(other.gameObject);
            _npcsCount--;
        }
    }

    private void SpawnNpc(){
        ItemData item = _itemsDatabase._itemsData[0]; //Todo make it random when there is more items
        var npc = Instantiate(_npcPrefab, _spawnPoint.position, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(item, false, _despawnPointPos, _despawnInShop,_windowPos, _doorPos, null, _counterPos);
        _npcsCount++;
        Debug.Log("Npc created | Item: " + item.ID + ", " + item.Name);
    }

    public void SpawnNpcInsideShop(Vector3 spawnPoint, ItemData item, GameObject display){
        var npc = Instantiate(_npcPrefab, spawnPoint, Quaternion.identity);
        npc.GetComponent<NpcBehaviour>().Initialize(item, true, _despawnPointPos, _despawnInShop, _windowPos, _doorPos, display, _counterPos);
    }

    public void DespawnNpc(GameObject npc){
        Destroy(npc);
        _npcsCount--;
    }

    private IEnumerator SpawnNpcDelay(){
        yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
        SpawnNpc();
    }
}
