using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InnerList<T>
{
    public List<T> scenario = new List<T>();
}


[System.Serializable]
public struct DailyNPCSpawns
{
    [SerializeField] private int _dayNr;
    [SerializeField] private List<InnerList<NPCGroupSpawn>> _partOfDayScenario;
    public int DayNr
    {
        get { return _dayNr; }
        private set { _dayNr = value; }
    }

    public List<InnerList<NPCGroupSpawn>> PartOfDayScenario
    {
        get { return _partOfDayScenario; }
        private set { _partOfDayScenario = value; }
    }
}

