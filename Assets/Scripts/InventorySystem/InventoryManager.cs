using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private GameObject[] _inventorySlots;
    private DisplaySlotController _currentDisplayDisplaySlot; // todo refactor and delete later
    private int _selectedInventorySlotId;

    public void SetNearestSlot(DisplaySlotController nearestDisplaySlot){
        _currentDisplayDisplaySlot = nearestDisplaySlot;
    }

    public void EnableInventory(){
        _inventoryUI.gameObject.SetActive(true);
    }

    public void AddItemToDisplaySlot(){ //On AddButton click
        var inventorySlotItemId = _inventorySlots[_selectedInventorySlotId].GetComponent<InventorySlot>().GetItemId();
        if (inventorySlotItemId != -1 && _currentDisplayDisplaySlot.GetItemId() == -1){
            var selectedItemIndex = _itemsDatabase._itemsData.FindIndex(data => data.ID == inventorySlotItemId);
            _currentDisplayDisplaySlot.PlaceItem(_itemsDatabase._itemsData[selectedItemIndex].Prefab, inventorySlotItemId);
            RemoveItemFromInventory();
        }
    }

    public void RemoveItemFromInventory(){
        _inventorySlots[_selectedInventorySlotId].GetComponent<InventorySlot>().RemoveItem();
        
        InventorySlot currentInventorySlot;
        InventorySlot previousInventorySlot = null;

        for (int i = 0; i < _inventorySlots.Length; i++){
            currentInventorySlot = _inventorySlots[i].GetComponent<InventorySlot>();
            if (i != 0)
                previousInventorySlot = _inventorySlots[i - 1].GetComponent<InventorySlot>();
                
            if (i != 0 && currentInventorySlot.GetItemId() != -1 &&
                previousInventorySlot.GetItemId() == -1){
                previousInventorySlot.AddItem(currentInventorySlot.GetItemId(), currentInventorySlot.GetImage().sprite);
                currentInventorySlot.RemoveItem();
            }
        }
    }

    public void Exit(){ //On ExitButton click
        _inventoryUI.gameObject.SetActive(false);
    }

    public void SelectSlot(int selectedSlotId){ //On inventory slot button click
        _selectedInventorySlotId = selectedSlotId;
    }

    public void RemoveItemFromDisplaySlot(){
        int itemId = _currentDisplayDisplaySlot.GetItemId();
        _currentDisplayDisplaySlot.RemoveItem();
        AddItemToInventory(itemId);
    }

    public void AddItemToInventory(int itemId){
        var selectedItemIndex = _itemsDatabase._itemsData.FindIndex(data => data.ID == itemId);
        foreach (var inventorySlot in _inventorySlots){
            if (inventorySlot.GetComponent<InventorySlot>().IsEmpty()){
                inventorySlot.GetComponent<InventorySlot>().AddItem(itemId, _itemsDatabase._itemsData[selectedItemIndex].PreviewImage);
                break;
            }
        }
    }

    public void AddSwordToInventory(){ //Method for tests, on addSwordButton click
        AddItemToInventory(0);
    }

    public void GetSlotsData(){ //for debug - shows item ids for every inventory slot on the console
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots){
            Debug.Log(counter + ": " + inventorySlot.GetComponent<InventorySlot>().GetItemId());
            counter++;
        }
    }
    
    
}
