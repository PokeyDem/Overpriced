using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIManager : SingletonDontDestroyOnLoad<SaveUIManager>{
    [SerializeField] private GameObject _saveUI;
    [SerializeField] private GameObject _saveSlotsPanel;
    private int _selectedSaveSlot;
    private List<SaveSlot> _saveSlots;

    private new void Awake(){
        base.Awake();
        _saveSlots = new List<SaveSlot>(_saveSlotsPanel.GetComponentsInChildren<SaveSlot>());

        int buttonID = 0;
        foreach (var button  in _saveSlots){
            var id = buttonID;
            button.GetComponent<Button>().onClick.AddListener(() => SetSelectedSaveSlotId(id));
            button.GetComponent<Button>().onClick.AddListener(EnableSelectedSlotOutline);
            button.SetId(id);
           buttonID++;
        }
    }

    public void EnableUI(){
        _saveUI.SetActive(true);
    }

    public void DisableUI(){
        _saveUI.SetActive(false);
    }

    public void OnSaveButtonClick(){
        SaveManager.Instance.SaveGame(_selectedSaveSlot);
        _saveSlots[_selectedSaveSlot].SetSaveInfo();
    }

    public void OnLoadButtonClick(){
        if (_saveSlots[_selectedSaveSlot].IsEmpty()){
            return;
        }
        SaveManager.Instance.LoadGame(_selectedSaveSlot);
    }

    private void SetSelectedSaveSlotId(int id){
        _selectedSaveSlot = id;
    }

    public int GetSelectedSaveSlotId(){
        return _selectedSaveSlot;
    }

    public void EnableSelectedSlotOutline(){
        foreach (var saveSlot in _saveSlots){
            saveSlot.DisableOutline();
        }
        _saveSlots[_selectedSaveSlot].EnableOutline();
    }
}
