using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private GameObject[] _inventorySlots;
    private SlotController _currentDisplaySlot; // todo refactor and delete later
    private int _selectedInventorySlotId;
    
    private void Awake(){
        _inventoryUI.enabled = false;
        
    }

    public void EnableInventory(SlotController slot){
        _inventoryUI.enabled = true;
        _currentDisplaySlot = slot;
    }

    public void AddItemToDisplaySlot(){ //On AddButton click
        var inventorySlotItemId = _inventorySlots[_selectedInventorySlotId].GetComponent<InventorySlot>().GetItemId();
        if (inventorySlotItemId != -1){
            var selectedItemIndex = _itemsDatabase._itemsData.FindIndex(data => data.ID == inventorySlotItemId);
            _currentDisplaySlot.PlaceItem(_itemsDatabase._itemsData[selectedItemIndex].Prefab, inventorySlotItemId);
        }
    }

    public void Exit(){ //On ExitButton click
        _inventoryUI.enabled = false;
    }

    public void SelectSlot(int selectedSlotId){ //On inventory slot button click
        _selectedInventorySlotId = selectedSlotId;
    }

    public void RemoveItemFromDisplaySlot(){
        _currentDisplaySlot.RemoveItem();
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
