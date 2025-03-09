using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private GameObject _itemSlotPrefab;
    [SerializeField] private GameObject _inventorySlotsContainer;
    private List<GameObject> _inventorySlots;
    private DisplaySlotController _currentDisplayDisplaySlot; // todo refactor and delete later
    private InventorySlot _selectedInventorySlot;

    public void Awake(){
        _inventorySlots = new List<GameObject>();
        AddSlots(15);
    }

    public void SetNearestSlot(DisplaySlotController nearestDisplaySlot){
        _currentDisplayDisplaySlot = nearestDisplaySlot;
    }

    public void AddSlot(){
        GameObject slot = Instantiate(_itemSlotPrefab, _inventorySlotsContainer.transform);
        _inventorySlots.Add(slot);
        int slotIndex = _inventorySlots.Count - 1;
        slot.GetComponentInChildren<Button>().onClick.AddListener(() => SelectSlot(slotIndex));
    }

    private void AddSlots(int amount){
        for (int i = 0; i < amount; i++){
            AddSlot();
        }
    }

    public void EnableInventory(){
        _inventoryUI.gameObject.SetActive(true);
        SelectSlot(0);
        _selectedInventorySlot.EnableOutline();
    }

    public void AddItemToDisplaySlot(){ //On AddButton click
        var inventorySlotItemId = _selectedInventorySlot.GetItemId();
        
        if (inventorySlotItemId != -1 && _currentDisplayDisplaySlot.GetItemId() == -1){
            var selectedItemIndex = _itemsDatabase._itemsData.FindIndex(data => data.ID == inventorySlotItemId);
            _currentDisplayDisplaySlot.PlaceItem(_itemsDatabase._itemsData[selectedItemIndex].Prefab, inventorySlotItemId);
            RemoveItemFromInventory();
        }
    }

    public void RemoveItemFromInventory(){
        
        if (_selectedInventorySlot.IsEmpty())
            return;
        
        if (_selectedInventorySlot.GetItemQuantity() > 1)
            _selectedInventorySlot.DecreaseQuantity();
        else
            _selectedInventorySlot.RemoveItem();
            
    }

    public void Exit(){ //On ExitButton click
        _inventoryUI.gameObject.SetActive(false);
    }

    public void SelectSlot(int selectedSlotId){ //On inventory slot button click
        
        if (_selectedInventorySlot) 
            _selectedInventorySlot.DisableOutline();
        
        _selectedInventorySlot = _inventorySlots[selectedSlotId].GetComponent<InventorySlot>();
        _selectedInventorySlot.EnableOutline();

        int currentItemId = _inventorySlots[selectedSlotId].GetComponent<InventorySlot>().GetItemId();
        if (currentItemId != -1){
            _itemInfoPanel.SetActive(true);
            Debug.Log(GetItemInfo(currentItemId));
            _itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(currentItemId);
        }
        else{
            _itemInfoPanel.SetActive(false);
        }
    }

    public string GetItemInfo(int index){
        ItemData currentItemData = _itemsDatabase._itemsData.Find(data => data.ID == index);
        return "Name: " + currentItemData.Name
            + "\nPrice: " + currentItemData.FinalPrice
            + "\nRarity: " + currentItemData.Rarity
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
        else{
            AddSlots(5);
            AddItemToInventory(itemId);
        }
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
