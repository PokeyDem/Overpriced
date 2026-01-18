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
    public List<int> itemIDs;

    public DisplayData(List<int> itemIDs){
        this.itemIDs = itemIDs;
    }
}

public class PurchasableDisplayData
{
    public List<int> itemIDs;
    public List<bool> isBought;

    public PurchasableDisplayData(List<bool> isBought, List<int> itemIDs)
    {
        this.itemIDs = itemIDs;
        this.isBought = isBought;
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
    public DisplayData DisplayData;
    public PurchasableDisplayData PurchasableDisplayData;
    public InventoryData InventoryData;
    public DayData DayData;
    public ShopStateData ShopStateData;
    public MoneyData MoneyData;
    public ExperienceData ExperienceData;

    public SaveData(PlayerData playerData, 
        InventoryData inventoryData, DayData dayData,
        ShopStateData shopStateData, MoneyData moneyData,
        ExperienceData experienceData, DisplayData displayData, PurchasableDisplayData purchasableDisplayData){
        PlayerData = playerData;
        InventoryData = inventoryData;
        DayData = dayData;
        ShopStateData = shopStateData;
        MoneyData = moneyData;
        ExperienceData = experienceData;
        DisplayData = displayData;
        PurchasableDisplayData = purchasableDisplayData;
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
