using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DailyNPCSpawns
{
    [SerializeField] private int _dayNr;
    [SerializeField] private List<NPCGroupSpawn> _npcBatchSpawn;
    public int DayNr
    {
        get { return _dayNr; }
        private set { _dayNr = value; }
    }
    public List<NPCGroupSpawn> NpcBatchSpawn
    {
        get { return _npcBatchSpawn; }
        private set { _npcBatchSpawn = value; }
    }
}
