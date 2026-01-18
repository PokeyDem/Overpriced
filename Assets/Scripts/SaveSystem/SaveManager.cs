using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : SingletonDontDestroyOnLoad<SaveManager>
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private ShopStateManager shopStateManager;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private ExperienceManager experienceManager;
    [SerializeField] private SaveUIManager saveUIManager;
    [SerializeField] private ItemsDatabaseSO itemsDatabase;
    private IDataService _dataService = new JsonDataService();
    private String QUICK_SAVE_PATH = "/quick_save.json";
    private String AUTO_SAVE_PATH = "/auto_save.json";
    private String SAVE_INFO_PATH = "/save_info.json";
    private String TEMP_SAVE_PATH = "/temp_save.json";

    private new void Awake(){
        base.Awake();
    }

    public void SaveGameToFile(string path){
        PlayerData playerData = player.GetPlayerData();
        DisplayData displayData = DisplaysManager.Instance.GetDisplaysData();
        PurchasableDisplayData purchasableDisplayData = DisplaysManager.Instance.GetPurchasableDisplayData();
        InventoryData inventoryData = inventoryManager.GetInventoryData();
        DayData dayData = dayManager.GetDayData();
        ShopStateData shopStateData = shopStateManager.GetShopStateData();
        MoneyData moneyData = moneyManager.GetMoneyData();
        ExperienceData experienceData = experienceManager.GetExperienceData();
        
        
        SaveData saveData = new SaveData(playerData,
            inventoryData, dayData, shopStateData, moneyData, experienceData, displayData, purchasableDisplayData);
        
        _dataService.SaveData(path, saveData, true);
    }

    public void LoadGameFromFile(string path){
        if (!_dataService.IsFileExists(path))
            return;
        
        SaveData saveData = _dataService.LoadData<SaveData>(path, true);
        player.LoadPlayer(saveData.PlayerData);
        DisplaysManager.Instance.LoadDisplaysData(saveData.DisplayData, saveData.PurchasableDisplayData);
        
        List<ItemData> itemDataList = new List<ItemData>();
        foreach (var item in saveData.InventoryData.InventoryItemsData) {
            itemDataList.Add(itemsDatabase.Find(data => data.ID==item.Key));
        }
        inventoryManager.LoadInventoryData(itemDataList);
        
        dayManager.LoadDayData(saveData.DayData);
        shopStateManager.LoadShopState(saveData.ShopStateData);
        moneyManager.LoadMoneyData(saveData.MoneyData);
        experienceManager.LoadExperienceData(saveData.ExperienceData);
        PauseMenuManager.Instance.Resume();
    }

    public void SaveGame(int slotID){
        if (slotID == 6)
            SaveGameToFile(QUICK_SAVE_PATH);
        else if (slotID == 7)
            SaveGameToFile(AUTO_SAVE_PATH);
        else
            SaveGameToFile($"/save_slot_{slotID}.json");
    }

    public void LoadGame(int slotID){
        if (slotID == 6)
            LoadGameFromFile(QUICK_SAVE_PATH);
        else if (slotID == 7)
            LoadGameFromFile(AUTO_SAVE_PATH);
        else
            LoadGameFromFile($"/save_slot_{slotID}.json");
    }

    public void SaveGameInTemporarySlot(){
        SaveGameToFile(TEMP_SAVE_PATH);
    }

    public void LoadGameFromTemporarySlot(){
        LoadGameFromFile(TEMP_SAVE_PATH);
    }

    public void SaveSlotsInfo(){
        SaveSlotsData saveSlotsData = SaveUIManager.Instance.GetSaveSlotsData();
        _dataService.SaveData(SAVE_INFO_PATH, saveSlotsData, true);
    }

    public void LoadSlotsInfo(){
        if (!_dataService.IsFileExists(SAVE_INFO_PATH)){
            Debug.Log("No save file found");
            return;
        }
        
        SaveSlotsData saveSlotsData =  _dataService.LoadData<SaveSlotsData>(SAVE_INFO_PATH, true);
        SaveUIManager.Instance.LoadSaveSlotsData(saveSlotsData);
    }
}
