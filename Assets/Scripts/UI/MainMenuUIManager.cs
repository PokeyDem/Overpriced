using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : SingletonWithDestroy<MainMenuUIManager>
{
   [SerializeField] private GameObject _settingsUI;
   [SerializeField] private GameObject _creditsUI;

   private new void Awake()
   {
      base.Awake();
   }

   public void ShowSettingsUI()
   {
      HideUI();
      _settingsUI.SetActive(true);
   }

   public void ShowCreditsUI()
   {
      HideUI();
      _creditsUI.SetActive(true);
   }
   
   private void HideUI()
   {
      _settingsUI.SetActive(false);
      _creditsUI.SetActive(false);
   }
}
