using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DoorPlayerTrigger : MonoBehaviour, IInteractable{
   public UnityEvent goOutsideEvent;
    public bool isActive;
    private void Awake()
    {
        isActive = true;
    }
    public void Interact()
    {
        goOutsideEvent?.Invoke();
    }

    public string TriggerInteractPrompt()
    {
        return "Go to Merchant Guild";
    }

    private void OnTriggerStay(Collider other){
      if (other.gameObject.CompareTag("Player") && ShopStateManager.ShopStateManagerInstance.ShopIsClose()){
            if (isActive)
            {
                other.gameObject.GetComponent<PlayerControl>().SetInteractable(this);
            } else other.gameObject.GetComponent<PlayerControl>().SetInteractable(null);
        }
   }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && ShopStateManager.ShopStateManagerInstance.ShopIsClose()&&isActive)
        {
            PlayerControl playerControl=other.gameObject.GetComponent<PlayerControl>();
            if(playerControl.GetInteractable() == this as IInteractable)
            {
                playerControl.SetInteractable(null);
            }
        }
    }
}
