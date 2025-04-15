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
    private IDataService _dataService = new JsonDataService();

    private new void Awake(){
        base.Awake();
    }
    public void SaveGameInFile(string path){
        PlayerData playerData = player.GetPlayerData();
        List<DisplayData> displayData = placementSystem.GetDisplayData();
        InventoryData inventoryData = inventoryManager.GetInventoryData();
        DayData dayData = dayManager.GetDayData();
        ShopStateData shopStateData = shopStateManager.GetShopStateData();
        MoneyData moneyData = moneyManager.GetMoneyData();
        ExperienceData experienceData = experienceManager.GetExperienceData();
        
        SaveData saveData = new SaveData(playerData, displayData, 
            inventoryData, dayData, shopStateData, moneyData, experienceData);
        
        _dataService.SaveData(path, saveData, true);
    }

    public void LoadGameFromFile(string path){
        SaveData saveData = _dataService.LoadData<SaveData>(path, true);
        player.LoadPlayer(saveData.PlayerData);
        placementSystem.LoadDisplayData(saveData.DisplayData);
        inventoryManager.LoadInventoryData(saveData.InventoryData.InventoryItemsData);
        dayManager.LoadDayData(saveData.DayData);
        shopStateManager.LoadShopState(saveData.ShopStateData);
        moneyManager.LoadMoneyData(saveData.MoneyData);
        experienceManager.LoadExperienceData(saveData.ExperienceData);
        PauseMenuManager.Instance.Resume();
    }

    public void SaveGame(int slotID){
        if (slotID == 6)
            SaveGameInFile($"/quick_save.json");
        else if (slotID == 7)
            SaveGameInFile("/auto_save.json");
        else
            SaveGameInFile($"/save_slot_{slotID}.json");
    }

    public void LoadGame(int slotID){
        if (slotID == 6)
            LoadGameFromFile($"/quick_save.json");
        else if (slotID == 7)
            LoadGameFromFile("/auto_save.json");
        else
            LoadGameFromFile($"/save_slot_{slotID}.json");
    }
}
