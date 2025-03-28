using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCGroupSpawn
{
    [SerializeField] private NPCType _npcType;
    [SerializeField] private int _spawnCount;

    public NPCType NpcType
    {
        get { return _npcType; }
        private set { _npcType = value; }
    }
    public int SpawnCount
    {
        get { return _spawnCount; }
        private set { _spawnCount = value; }
    }
}
