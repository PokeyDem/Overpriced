using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopTriggerBehaviour : MonoBehaviour{ //Only for tests
   [SerializeField] private Canvas _ShopUI;
   [SerializeField] private Canvas _InventoryUI;
   [SerializeField] private List<Button> _buttonsToDisable;

   private void OnTriggerEnter(Collider other){
      if (other.CompareTag("Player")){
         EnableUI();
      }
   }

   private void OnTriggerExit(Collider other){
      if (other.CompareTag("Player")){
         DisableUI();
      }
   }

   private void EnableUI(){
      _ShopUI.gameObject.SetActive(true);
      _InventoryUI.gameObject.SetActive(true);
      foreach (Button button in _buttonsToDisable){
         button.gameObject.SetActive(false);
      }
   }

   private void DisableUI(){
      _ShopUI.gameObject.SetActive(false);
      _InventoryUI.gameObject.SetActive(false);
      foreach (Button button in _buttonsToDisable){
         button.gameObject.SetActive(true);
      }
   }
}
