using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DoorPlayerTrigger : MonoBehaviour{
   public UnityEvent goOutsideEvent;

   private void OnTriggerEnter(Collider other){
      if (other.CompareTag("Player") && ShopStateManager.ShopStateManagerInstance.ShopIsClose()){
         goOutsideEvent.Invoke();
      }
   }
}
