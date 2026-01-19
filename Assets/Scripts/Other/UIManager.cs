using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonDontDestroyOnLoad<UIManager>
{
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _cityUI;
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private GameObject _merchantsGuildUI;
    [SerializeField] private GameObject _buildingInfoPanel;
    [SerializeField] private Vector3 _buildingInfoPanelOffset;
    [SerializeField] private List<TextMeshProUGUI> _mainMenuButtonsText;
    [SerializeField] private GameObject _endScreenMainMenuButton;
    [SerializeField] private GameObject _endScreenWinText;
    [SerializeField] private GameObject _endScreenLoseText;
    [SerializeField] private GameObject _tutorialHudUI;

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
        DisableTutorialUI();
    }

    public void EnableHUD(){
        _hud.SetActive(true);
        EnableTutorialUI();
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

    public void ShowEndScreenUIElements(bool hasWon)
    {
        _endScreenMainMenuButton.SetActive(true);
        
        if (hasWon)
            _endScreenWinText.SetActive(true);
        else
            _endScreenLoseText.SetActive(true);
    }

    public void HideEndScreenUIElements()
    {
        _endScreenMainMenuButton.SetActive(false);
        
        _endScreenWinText.SetActive(false);
    
        _endScreenLoseText.SetActive(false);
    }

    private void DisableTutorialUI()
    {
        _tutorialHudUI.SetActive(false);
    }

    private void EnableTutorialUI()
    {
        _tutorialHudUI.SetActive(true);
    }
}
