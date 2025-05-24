using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonDontDestroyOnLoad<UIManager>{
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _cityUI;
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private GameObject _merchantsGuildUI;

    private new void Awake(){
        base.Awake();
    }

    public void EnableMerchantsGuildUI(){
        _merchantsGuildUI.SetActive(true);
    }

    public void DisableMerchantsGuildUI(){
        _merchantsGuildUI.SetActive(false);
    }

    public void SwitchToCityUI(){
        DisableHUD();
        EnableCityUI();
        DisableInteractUI();
    }

    public void SwitchToShopUI(){
        EnableHUD();
        DisableCityUI();
        EnableInteractUI();
    }
    
    private void DisableHUD(){
        _hud.SetActive(false);
    }

    private void EnableHUD(){
        _hud.SetActive(true);
    }

    private void DisableCityUI(){
        _cityUI.SetActive(false);
    }

    private void EnableCityUI(){
        _cityUI.SetActive(true);
    }

    private void EnableInteractUI(){
        _interactUI.SetActive(true);
    }

    private void DisableInteractUI(){
        _interactUI.SetActive(false);
    }


}
