using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private ShopStateManager shopStateManager;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private ExperienceManager experienceManager;
    [SerializeField] private ItemsDatabaseSO itemsDatabase;
     private IDataService _dataService = new JsonDataService();

    public void SaveGame()
    {
        PlayerData playerData = player.GetPlayerData();
        List<DisplayData> displayData = placementSystem.GetDisplayData();
        InventoryData inventoryData = inventoryManager.GetInventoryData();
        DayData dayData = dayManager.GetDayData();
        ShopStateData shopStateData = shopStateManager.GetShopStateData();
        MoneyData moneyData = moneyManager.GetMoneyData();
        ExperienceData experienceData = experienceManager.GetExperienceData();
        
        SaveData saveData = new SaveData(playerData, displayData, 
            inventoryData, dayData, shopStateData, moneyData, experienceData);
        
        _dataService.SaveData("/save.json", saveData, true);
    }

    public void LoadGame(){
        SaveData saveData = _dataService.LoadData<SaveData>("/save.json", true);
        player.LoadPlayer(saveData.PlayerData);
        placementSystem.LoadDisplayData(saveData.DisplayData);
        
        List<ItemData> itemDataList = new List<ItemData>();
        foreach (var item in saveData.InventoryData.InventoryItemsData) {
            itemDataList.Add(itemsDatabase._itemsData.Find(data => data.ID==item.Key));
        }
        inventoryManager.LoadInventoryData(itemDataList);
        
        dayManager.LoadDayData(saveData.DayData);
        shopStateManager.LoadShopState(saveData.ShopStateData);
        moneyManager.LoadMoneyData(saveData.MoneyData);
        experienceManager.LoadExperienceData(saveData.ExperienceData);
    }

    // public void LoadGame()
    // {
    //     GameData data = saveSystem.LoadGame();
    //     if (data != null)
    //     {
    //         // Load player data
    //         player.LoadPlayerData(data.player);
    //
    //         // Load display data
    //         foreach (Display display in displays)
    //         {
    //             DisplayData savedDisplay = data.displays.Find(d => d.displayID == display.displayID);
    //             if (savedDisplay != null)
    //             {
    //                 display.LoadDisplayData(savedDisplay);
    //             }
    //         }
    //     }
    // }
}
