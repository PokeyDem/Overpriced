using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerBeahviour : MonoBehaviour{
    [SerializeField] private Transform _doorSpawnPoint;

    [SerializeField] private NpcManager _npcManager;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPC")){
            NpcBehaviour npcBehaviour = other.gameObject.GetComponent<NpcBehaviour>();
            _npcManager.SpawnNpcInsideShop(_doorSpawnPoint.position, npcBehaviour.GetItemToBuy(), npcBehaviour.GetDisplayItemSlot(), npcBehaviour.GetNpcType());
            Destroy(other.gameObject);
        }
    }
}
