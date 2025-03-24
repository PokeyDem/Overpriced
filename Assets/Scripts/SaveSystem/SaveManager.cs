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
    private IDataService _dataService = new JsonDataService();

    public void SaveGame()
    {
        PlayerData playerData = player.GetPlayerData();
        List<DisplayData> displayData = placementSystem.GetDisplayData();
        InventoryData inventoryData = inventoryManager.GetInventoryData();
        DayData dayData = dayManager.GetDayData();
        ShopStateData shopStateData = shopStateManager.GetShopStateData();
        MoneyData moneyData = moneyManager.GetMoneyData();
        
        SaveData saveData = new SaveData(playerData, displayData, 
            inventoryData, dayData, shopStateData, moneyData);
        
        _dataService.SaveData("/save.json", saveData, true);
    }

    public void LoadGame(){
        SaveData saveData = _dataService.LoadData<SaveData>("/save.json", true);
        player.LoadPlayer(saveData.PlayerData);
        placementSystem.LoadDisplayData(saveData.DisplayData);
        inventoryManager.LoadInventoryData(saveData.InventoryData.InventoryItemsData);
        dayManager.LoadDayData(saveData.DayData);
        shopStateManager.LoadShopState(saveData.ShopStateData);
        moneyManager.LoadMoneyData(saveData.MoneyData);
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
