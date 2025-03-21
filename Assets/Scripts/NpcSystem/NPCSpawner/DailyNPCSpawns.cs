using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DailyNPCSpawns
{
    [SerializeField] private int _dayNr;
    [SerializeField] private List<NPCGroupSpawn> _morningScenario;
    [SerializeField] private List<NPCGroupSpawn> _afternoonScenario;
    public int DayNr
    {
        get { return _dayNr; }
        private set { _dayNr = value; }
    }
    public List<NPCGroupSpawn> MorningScenario
    {
        get { return _morningScenario; }
        private set { _morningScenario = value; }
    }
    public List<NPCGroupSpawn> AfternoonScenario
    {
        get { return _afternoonScenario; }
        private set { _afternoonScenario = value; }
    }
}
