using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIManager : SingletonDontDestroyOnLoad<SaveUIManager>{
    [SerializeField] private GameObject _saveUI;
    [SerializeField] private GameObject _saveSlotsPanel;
    [SerializeField] private GameObject _quickSaveSlot;
    [SerializeField] private GameObject _autoSaveSlot;
    [SerializeField] private GameObject _saveButton;
    [SerializeField] private List<GameObject> _mainMenuSaveButtons;
    private int _selectedSaveSlot;
    private int _quickSaveSlotID = 6;
    private int _autoSaveSlotID = 7;
    private List<SaveSlot> _saveSlots;

    private new void Awake(){
        base.Awake();
       InitializeButtons();
    }

    public void InitializeButtons(){
        _saveSlots = new List<SaveSlot>(_saveSlotsPanel.GetComponentsInChildren<SaveSlot>());
        _saveSlots.Add(_quickSaveSlot.GetComponent<SaveSlot>());
        _saveSlots.Add(_autoSaveSlot.GetComponent<SaveSlot>());
        int buttonID = 0;
        foreach (var button in _saveSlots){
            var id = buttonID;
            button.GetComponent<Button>().onClick.AddListener(() => SetSelectedSaveSlotId(id));
            button.GetComponent<Button>().onClick.AddListener(EnableSelectedSlotOutline);
            button.SetId(id);
            buttonID++;
        }
    }
    
    public void DisableMainMenuSaveButtons(){
        foreach (var button in _mainMenuSaveButtons){
            button.SetActive(false);
        }
    }
    
    public void EnableMainMenuSaveButtons(){
        foreach (var button in _mainMenuSaveButtons){
            button.SetActive(true);
        }
    }

    public SaveSlotsData GetSaveSlotsData(){
        List<String> data = new List<string>();

        foreach (var slot in _saveSlots){
            data.Add(slot.GetSaveInfo());
        }
        
        return new SaveSlotsData(data);
    }

    public void LoadSaveSlotsData(SaveSlotsData data){
        for (int i = 0; i < _saveSlots.Count; i++){
            _saveSlots[i].InitializeButton();
            _saveSlots[i].SetSaveInfo(data.SlotsData[i]);
        }
    }

    public void EnableUI(){
        _saveUI.SetActive(true);
    }

    public void DisableUI(){
        _saveUI.SetActive(false);
    }

    public void OnSaveButtonClick(){
        _saveSlots[_selectedSaveSlot].SetSaveInfo();
        SaveManager.Instance.SaveGame(_selectedSaveSlot);
    }

    public void OnQuickSaveButtonClick(){
        _saveSlots[_quickSaveSlotID].InitializeButton();
        SaveManager.Instance.SaveGame(_quickSaveSlotID);
        _saveSlots[_quickSaveSlotID].SetSaveInfo();
    }

    public void OnQuickLoadButtonClick(){
        if (_saveSlots[_quickSaveSlotID].IsEmpty()){
            return;
        }
        SaveManager.Instance.LoadGame(_quickSaveSlotID);
    }

    public void DisableSaveButton(){
        _saveButton.gameObject.SetActive(false);
    }

    public void OnAutoSaveButtonClick(){
        _saveSlots[_autoSaveSlotID].InitializeButton();
        SaveManager.Instance.SaveGame(_autoSaveSlotID);
        _saveSlots[_autoSaveSlotID].SetSaveInfo();
    }

    public void OnLoadButtonClick(){
        if (_saveSlots[_selectedSaveSlot].IsEmpty()){
            return;
        }
        SaveManager.Instance.LoadGame(_selectedSaveSlot);
        
        if (_selectedSaveSlot == 7)
            _saveButton.gameObject.SetActive(true);
    }

    private void SetSelectedSaveSlotId(int id){
        _selectedSaveSlot = id;
        if (_selectedSaveSlot == 7)
            _saveButton.gameObject.SetActive(false);
        else if (!_saveButton.gameObject.activeSelf)
            _saveButton.gameObject.SetActive(true);
    }

    public int GetSelectedSaveSlotId(){
        return _selectedSaveSlot;
    }

    public void EnableSelectedSlotOutline(){
        Debug.Log("Enable selected slot outline");
        foreach (var saveSlot in _saveSlots){
            saveSlot.DisableOutline();
        }
        _saveSlots[_selectedSaveSlot].EnableOutline();
    }
}
