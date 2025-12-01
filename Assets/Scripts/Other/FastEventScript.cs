using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FastEventScript : MonoBehaviour {
    public UnityEvent fastColisionEnterEvent;
    public UnityEvent fastColisionExitEvent;
    
    private void OnTriggerEnter(Collider other){
        fastColisionEnterEvent.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        fastColisionExitEvent.Invoke();
    }
}
