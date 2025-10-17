using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerBeahviour : MonoBehaviour{
    [SerializeField] private Transform _doorSpawnPoint;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPC")){
            NpcBehaviour npcBehaviour = other.gameObject.GetComponent<NpcBehaviour>();
            //npcBehaviour.ReturnToPool();
            //_npcManager.SpawnNpcInsideShop(_doorSpawnPoint.position, npcBehaviour.GetDisplaySlotController(), npcBehaviour.GetNpcType());
            //npcBehaviour.WarpNPC(_doorSpawnPoint.position);
            //npcBehaviour.StartBrowsing();
        }
    }
}
