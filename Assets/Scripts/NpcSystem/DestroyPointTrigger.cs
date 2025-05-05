using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DestroyPointTrigger : MonoBehaviour{

    [FormerlySerializedAs("_npcSpawner")] [SerializeField] private NpcManager npcManager;
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPC")){
            npcManager.DespawnNpc(other.gameObject.GetComponent<NpcBehaviour>());
        }
    }
}
