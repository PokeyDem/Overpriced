using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DoorPlayerTrigger : MonoBehaviour{
   public UnityEvent goOutsideEvent;
   private void OnCollisionEnter(Collision other){
      if (other.gameObject.CompareTag("Player") && ShopStateManager.ShopStateManagerInstance.ShopIsClose()){
         goOutsideEvent.Invoke();
      }
   }
}
