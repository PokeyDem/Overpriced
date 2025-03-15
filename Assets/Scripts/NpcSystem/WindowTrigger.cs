using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTrigger : MonoBehaviour{
    private void OnTriggerEnter(Collider other){
        Debug.Log("triggered");
        if (other.CompareTag("NPC"))
            //other.GetComponent<NpcBehaviour>().CheckItemsOnDisplays();
            other.GetComponent<NpcBehaviour>().FindDesiredItemsInShop();
    }
}
