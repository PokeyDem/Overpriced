using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTrigger : MonoBehaviour{
    
    private Collider _collider;
    private bool _isTarget;

    private void Awake(){
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPC") && _isTarget){
            //StartCoroutine(other.GetComponent<NpcBehaviour>().GoToCheckout());
        }
    }

    public void SetIsEnabled(bool isEnabled){
        _collider.enabled = isEnabled;
    }
    public void SetIsTarget(bool isTarget)
    {
        _isTarget = isTarget;
    }
}
