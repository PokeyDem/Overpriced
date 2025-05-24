using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDoorBehaviour : MonoBehaviour, IInteractibleOnClick
{
   
   public void Interact(){
      UIManager.Instance.EnableMerchantsGuildUI();
   }
}
