using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaysManager : SingletonWithDestroy<DisplaysManager>
{
    [SerializeField] private DisplaySlotController[] _slots;
    [SerializeField] private DisplaySlotController[] _purchasableSlots;
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;


    private new void Awake()
    {
        base.Awake();
    }

    public void ResetDisplays()
    {
        foreach (var slot in _slots)
        {
            slot.RemoveItem();
        }

        foreach (var slot in _purchasableSlots)
        {
            slot.RemoveItem();
            slot.ResetPurchasableDisplay();
        }
    }

    public DisplayData GetDisplaysData()
    {
        List<int> ids = new List<int>();

        foreach (var displaySlotController in _slots)
        {
            if (displaySlotController.ItemData != null)
                ids.Add(displaySlotController.ItemData.ID);
            else
                ids.Add(-1);
        }

        return new DisplayData(ids);
    }

    public PurchasableDisplayData GetPurchasableDisplayData()
    {
        List<int> ids = new List<int>();
        List<bool> isBought = new List<bool>();

        foreach (var displaySlotController in _purchasableSlots)
        {
            if (displaySlotController.ItemData != null)
                ids.Add(displaySlotController.ItemData.ID);
            else
                ids.Add(-1);
            
            isBought.Add(displaySlotController.IsBought);
        }

        return new PurchasableDisplayData(isBought, ids);
    }

    public void LoadDisplaysData(DisplayData displayData, PurchasableDisplayData  purchasableDisplayDataData)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].RemoveItem();
            _slots[i].PlaceItem(_itemsDatabase.Find(data =>data.ID==displayData.itemIDs[i]));
        }

        for (int i = 0; i < _purchasableSlots.Length; i++)
        {
            if (!purchasableDisplayDataData.isBought[i] && _purchasableSlots[i].IsBought)
            {
                _purchasableSlots[i].RemoveItem();
                _purchasableSlots[i].ResetPurchasableDisplay();
            }

            if (purchasableDisplayDataData.isBought[i])
            {
                _purchasableSlots[i].RemoveItem();
                _purchasableSlots[i].PlaceItem(_itemsDatabase.Find(data => data.ID==purchasableDisplayDataData.itemIDs[i]));
            }
        }
    }
}
