using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData{
    public float x, y, z;

    public PlayerData(float posX, float posY, float posZ){
        x = posX;
        y = posY;
        z = posZ;
    }
}

public class DisplayData{
    public int displayId;
    public float x, y, z;
    public List<int> itemsID;

    public DisplayData(int _displayId, float posX, float posY, float posZ, List<int> _itemsID){
        displayId = _displayId;
        x = posX;
        y = posY;
        z = posZ;
        itemsID = _itemsID;
    }
}

public class InventoryData{
    public Dictionary<int, int> InventoryItemsData;

    public InventoryData(Dictionary<int, int> inventoryItemsData){
        InventoryItemsData = inventoryItemsData;
    }
}

public class SaveData{
    public PlayerData PlayerData;
    public List<DisplayData> DisplayData;
    public InventoryData InventoryData;
    public DayData DayData;
    public ShopStateData ShopStateData;
    public MoneyData MoneyData;
    public ExperienceData ExperienceData;
    public SaveSlotsData SaveSlotsData;

    public SaveData(PlayerData playerData, List<DisplayData> displayData, 
        InventoryData inventoryData, DayData dayData,
        ShopStateData shopStateData, MoneyData moneyData,
        ExperienceData experienceData){
        PlayerData = playerData;
        DisplayData = displayData;
        InventoryData = inventoryData;
        DayData = dayData;
        ShopStateData = shopStateData;
        MoneyData = moneyData;
        ExperienceData = experienceData;
    }
}

public class DayData{
    public int DayCount;
    public DayManager.PartOfDay DayPart;

    public DayData(int dayCount, DayManager.PartOfDay dayPart){
        DayCount = dayCount;
        DayPart = dayPart;
    }
}

public class ShopStateData{
    public ShopStateManager.ShopState ShopState;
    public int OpenShopTime;

    public ShopStateData(ShopStateManager.ShopState shopState, int openShopTime){
        ShopState = shopState;
        OpenShopTime = openShopTime;
    }
}

public class MoneyData{
    public int Money;

    public MoneyData(int money){
        Money = money;
    }
}

public class ExperienceData{
    public int Level;
    public float CurrentExp;

    public ExperienceData(int level, float currentExp){
        Level = level;
        CurrentExp = currentExp;
    }
}

public class SaveSlotsData{
    public List<String> SlotsData;

    public SaveSlotsData(List<String> slotsData){
        SlotsData = slotsData;
    }
}
