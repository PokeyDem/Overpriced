using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonDontDestroyOnLoad<UIManager>{
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _cityUI;
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private GameObject _merchantsGuildUI;
    [SerializeField] private GameObject _buildingInfoPanel;
    [SerializeField] private Vector3 _buildingInfoPanelOffset;
    [SerializeField] private List<TextMeshProUGUI> _mainMenuButtonsText;

    private new void Awake(){
        base.Awake();
    }

    public void SetBuildingInfoPanelPosition(Vector3 newPos){
        _buildingInfoPanel.gameObject.transform.position = new Vector3(newPos.x + _buildingInfoPanelOffset.x,
            newPos.y + _buildingInfoPanelOffset.y, newPos.z + _buildingInfoPanelOffset.z);
    }
    
    public void SetBuildingInfo(string info){
        _buildingInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = info;
    }

    public void EnableBuildingInfoPanel(){
        _buildingInfoPanel.SetActive(true);
    }

    public void DisableBuildingInfoPanel(){
        _buildingInfoPanel.SetActive(false);
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
    
    public void DisableHUD(){
        _hud.SetActive(false);
    }

    public void EnableHUD(){
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

    public void HideMainMenuButtons()
    {
        foreach (var buttonText in _mainMenuButtonsText)
        {
            buttonText.gameObject.SetActive(false);
        }
    }

    public void ShowMainMenuButtons()
    {
        foreach (var buttonText in _mainMenuButtonsText)
        {
            buttonText.gameObject.SetActive(true);
        }
    }
}
