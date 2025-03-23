using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCGroupSpawn
{
    [SerializeField] private GameObject _npcPrefab;
    [SerializeField] private int _spawnCount;

    public GameObject NpcPrefab
    {
        get { return _npcPrefab; }
        private set { _npcPrefab = value; }
    }
    public int SpawnCount
    {
        get { return _spawnCount; }
        private set { _spawnCount = value; }
    }
}
