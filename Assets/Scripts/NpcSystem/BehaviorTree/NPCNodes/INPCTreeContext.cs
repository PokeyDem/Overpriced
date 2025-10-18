using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface INPCTreeContext
{
    public NavMeshAgent Agent { get;}
    public Transform DespawnPointPos { get;}
    public Transform WindowPos { get;}
    public Transform DoorPos { get;}
    public Transform DespawnInShop { get;}
    public Transform ShopSpawnPoint { get;}
    public List<Transform> CounterPos { get;}
    public List<ItemData> DesiredItems { get;}

    public Vector3 Target { get; set; }
    public DisplaySlotController DisplaySlotController { get; set; }

    public event Action Initialized;
    public event Action DecidingStarted;
    public event Action ItemRejected;
    public event Action ItemSelected;
}
