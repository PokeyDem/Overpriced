using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private GameObject[] _inventorySlots;
    [SerializeField] private Image _inventorySlotIndicator;
    [SerializeField] private GameObject _itemInfoPanel;
    private DisplaySlotController _currentDisplayDisplaySlot; // todo refactor and delete later
    private int _selectedInventorySlotId;
    

    public void SetNearestSlot(DisplaySlotController nearestDisplaySlot){
        _currentDisplayDisplaySlot = nearestDisplaySlot;
    }

    public void EnableInventory(){
        _inventoryUI.gameObject.SetActive(true);
        SelectSlot(0);
        _inventorySlotIndicator.gameObject.SetActive(true);
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
        
        InventorySlot inventorySlot = _inventorySlots[_selectedInventorySlotId].GetComponent<InventorySlot>();
        
        if (inventorySlot.IsEmpty())
            return;
        
        if (inventorySlot.GetItemQuantity() > 1)
            inventorySlot.DecreaseQuantity();
        else
            inventorySlot.RemoveItem();
            
    }

    public void Exit(){ //On ExitButton click
        _inventoryUI.gameObject.SetActive(false);
    }

    public void SelectSlot(int selectedSlotId){ //On inventory slot button click
        _selectedInventorySlotId = selectedSlotId;
        _inventorySlotIndicator.transform.position = _inventorySlots[selectedSlotId].transform.position;

        int currentItemId = _inventorySlots[selectedSlotId].GetComponent<InventorySlot>().GetItemId();
        if (currentItemId != -1){
            _itemInfoPanel.SetActive(true);
            _itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(currentItemId);
        }
        else{
            _itemInfoPanel.SetActive(false);
        }
    }

    public string GetItemInfo(int index){
        ItemData currentItemData = _itemsDatabase._itemsData.Find(data => data.ID == index);
        return "Name: " + currentItemData.Name
            + "\nPrice: " + currentItemData.Price
            + "\nDescription: " + currentItemData.Description;
    }
    public void RemoveItemFromDisplaySlot(){
        int itemId = _currentDisplayDisplaySlot.GetItemId();
        _currentDisplayDisplaySlot.RemoveItem();
        AddItemToInventory(itemId);
    }

    public void AddItemToInventory(int itemId){
        int slotIndex = -1;
        if ((slotIndex = FindExistingItem(itemId)) != -1){
            _inventorySlots[slotIndex].GetComponent<InventorySlot>().IncreaseQuantity();
            Debug.Log("Found Same Item");
        }
        else if ((slotIndex = FindFreePosition()) != -1)
            _inventorySlots[slotIndex].GetComponent<InventorySlot>().AddItem(itemId, _itemsDatabase._itemsData[itemId].PreviewImage);
    }

    private int FindExistingItem(int index){
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots){
            if (inventorySlot.GetComponent<InventorySlot>().GetItemId().Equals(index)){
                return counter;
            }
            counter++;
        }
        return -1;
    }

    private int FindFreePosition(){
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots){
            if (inventorySlot.GetComponent<InventorySlot>().IsEmpty()){
                return counter;
            }
            counter++;
        }
        return -1;
    }

    public void AddSwordToInventory(){ //Method for tests, on addSwordButton click
        AddItemToInventory(0);
    }

    public void GetSlotsData(){ //for debug - shows items ids for every inventory slot on the console
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots){
            Debug.Log(counter + ": " + inventorySlot.GetComponent<InventorySlot>().GetItemId());
            counter++;
        }
    }
    
    
}
