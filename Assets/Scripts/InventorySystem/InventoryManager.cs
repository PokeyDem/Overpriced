using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private GameObject _itemSlotPrefab;
    [SerializeField] private GameObject _inventorySlotsContainer;
    [SerializeField] private MoneyManager _moneyManager; //todo delete when merchants guild is created
    [SerializeField] private RectTransform _contentPanelTransform;
    
    private List<GameObject> _inventorySlots;
    private Dictionary<int, int> _inventorySlotsDictionary; //<itemId, inventorySlotIndex>
    private Dictionary<ItemType, List<int>> _itemTypesDictionary; 
    
    private DisplaySlotController _currentDisplayDisplaySlot; // todo refactor and delete later
    private InventorySlot _selectedInventorySlot;
    private int _selectedSlotId;

    private ItemType _categoryToSortBy = ItemType.All;
    private ItemType[] _itemTypes; 
    private int _itemTypeIndex = 0;

    [SerializeField] private TextMeshProUGUI _itemCategoryText; //Text for the sort by category button
    
    private const int DEFAULT_SLOT_COUNT = 15; //Todo move to config later

    public void Awake(){
        _inventorySlots = new List<GameObject>();
        _inventorySlotsDictionary = new Dictionary<int, int>();
        _itemTypesDictionary = new Dictionary<ItemType, List<int>>();
        InitItemTypesDictionary();
        _itemTypes = (ItemType[])Enum.GetValues(typeof(ItemType));
        AddSlots(DEFAULT_SLOT_COUNT);
        _itemCategoryText.text = ItemType.All.ToString();
    }

    private void ClearInventory(){
        foreach (var slot in _inventorySlots){
            if (slot.GetComponent<InventorySlot>().GetItemId() != -1) 
                slot.GetComponent<InventorySlot>().RemoveItem();
        }
        _inventorySlotsDictionary = new Dictionary<int, int>();
        _itemTypesDictionary = new Dictionary<ItemType, List<int>>();
        InitItemTypesDictionary();
    }

    public void SetNearestSlot(DisplaySlotController nearestDisplaySlot){
        _currentDisplayDisplaySlot = nearestDisplaySlot;
    }

    private void InitItemTypesDictionary(){
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType))){
            _itemTypesDictionary.Add(itemType, new List<int>());
        }
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

    public void DisableInventory(){
        _inventoryUI.gameObject.SetActive(false);
    }

    public void AddItemToDisplaySlot(){ //On AddButton click
        var inventorySlotItemId = _selectedInventorySlot.GetItemId();
        
        if (inventorySlotItemId != -1 && _currentDisplayDisplaySlot.GetItemId() == -1){
            var selectedItemIndex = _itemsDatabase._itemsData.FindIndex(data => data.ID == inventorySlotItemId);
            _currentDisplayDisplaySlot.PlaceItem(_itemsDatabase._itemsData[selectedItemIndex]);
            RemoveItemFromInventory();
        }
    }

    public void RemoveItemFromInventory(){
        ItemData itemToRemove = _itemsDatabase._itemsData[_selectedInventorySlot.GetItemId()];
        
        if (_selectedInventorySlot.IsEmpty())
            return;

        if (_selectedInventorySlot.GetItemQuantity() > 1){
            _selectedInventorySlot.DecreaseQuantity();
        }
        else{
            _inventorySlotsDictionary.Remove(_selectedInventorySlot.GetItemId());
            _itemTypesDictionary[itemToRemove.ItemType].Remove(_selectedSlotId);
            _selectedInventorySlot.RemoveItem();
            if (_categoryToSortBy != ItemType.All)
                _selectedInventorySlot.gameObject.SetActive(false);
        }
    }

    public void Exit(){ //On ExitButton click
        _inventoryUI.gameObject.SetActive(false);
    }

    public void SelectSlot(int selectedSlotId){ //On inventory slot button click

        _selectedSlotId = selectedSlotId;
        
        if (_selectedInventorySlot) 
            _selectedInventorySlot.DisableOutline();
        
        _selectedInventorySlot = _inventorySlots[selectedSlotId].GetComponent<InventorySlot>();
        _selectedInventorySlot.EnableOutline();

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
            + "\nPrice: " + currentItemData.FinalPrice
            + "\nRarity: " + new string(Convert.ToChar("*"), currentItemData.Rarity)
            + "\nDescription: " + currentItemData.Description;
    }
    public void RemoveItemFromDisplaySlot(){
        int itemId = _currentDisplayDisplaySlot.GetItemId();
        
        if (itemId == -1) return;
        
        _currentDisplayDisplaySlot.RemoveItem();
        AddItemToInventory(itemId);
    }

    public void AddItemToInventory(int itemId){
        int slotIndex = -1;
        ItemData itemToAdd = _itemsDatabase._itemsData.Find(data => data.ID == itemId);

        if ((slotIndex = FindExistingItem(itemId)) != -1){
            _inventorySlots[slotIndex].GetComponent<InventorySlot>().IncreaseQuantity();
            _itemTypesDictionary[itemToAdd.ItemType].Add(slotIndex);
        }
        else if ((slotIndex = FindFreePosition()) != -1){
            _inventorySlots[slotIndex].GetComponent<InventorySlot>().AddItem(itemId, itemToAdd.PreviewImage, itemToAdd.Rarity);
            _inventorySlotsDictionary[itemToAdd.ID] = slotIndex;
            _itemTypesDictionary[itemToAdd.ItemType].Add(slotIndex);
        }
        else{
            AddSlots(5);
            IncreaseContentPanelSize();
            AddItemToInventory(itemId);
        }
        
        EnableSlotsForSpecificCategory(_categoryToSortBy);
    }

    private void AddRandomRarityItemToInventory(ItemType itemType){
        List<ItemData> matchedItems = _itemsDatabase._itemsData.FindAll(data => data.ItemType.Equals(itemType));
        ItemData randomItem = matchedItems[UnityEngine.Random.Range(0, matchedItems.Count)];
        if (_moneyManager.GetCurrentMoney() >= randomItem.FinalPrice){
            AddItemToInventory(randomItem.ID);
            _moneyManager.ReduceMoney(randomItem.FinalPrice);
        }
    }

    public void AddRandomWeaponToInventory(){
        AddRandomRarityItemToInventory(ItemType.Weapon);
    }

    public void AddRandomPotionToInventory(){
        AddRandomRarityItemToInventory(ItemType.Potion);
    }

    public void AddRandomFoodToInventory(){
        AddRandomRarityItemToInventory(ItemType.Food);
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

    private List<int> GetItemsByCategory(ItemType category){
        return _itemTypesDictionary.ContainsKey(category) ? _itemTypesDictionary[category] : new List<int>();
    }

    public void OnFilterByCategoryButtonPress(){
        _itemTypeIndex = (_itemTypeIndex + 1) % _itemTypes.Length; // Move to next index and loop back
        _categoryToSortBy = _itemTypes[_itemTypeIndex];
        _itemCategoryText.text = _categoryToSortBy.ToString();
        FilterInventoryByCategory(_categoryToSortBy);
    }
    private void FilterInventoryByCategory(ItemType itemCategory){

        if (itemCategory == ItemType.All){
            EnableAllInventorySlots();
        }else{
            
            DisableAllInventorySlots();

           EnableSlotsForSpecificCategory(itemCategory);
        } 
    }

    private void EnableSlotsForSpecificCategory(ItemType itemCategory){
        List<int> itemsInCategory = GetItemsByCategory(itemCategory);
        foreach (int itemIndex in itemsInCategory){
            _inventorySlots[itemIndex].gameObject.SetActive(true);
        } 
    }

    private void DisableAllInventorySlots(){
        foreach (var slot in _inventorySlots){
            slot.SetActive(false);
        }
    }

    private void EnableAllInventorySlots(){
        foreach (var slot in _inventorySlots){
            slot.SetActive(true);
        }
    }

    public void GetSlotsData(){ //for debug - shows items id for every inventory slot on the console
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots){
            Debug.Log(counter + ": " + inventorySlot.GetComponent<InventorySlot>().GetItemId());
            counter++;
        }
    }

    private void IncreaseContentPanelSize(){
        var delta = _contentPanelTransform.sizeDelta;
        delta.y += 30;
        _contentPanelTransform.sizeDelta = delta;
    }

    public InventoryData GetInventoryData(){
        return new InventoryData(_inventorySlotsDictionary);
    }

    public void LoadInventoryData(Dictionary<int, int> inventoryData){
        ClearInventory();
        foreach (var itemData in inventoryData){
            AddItemToInventory(itemData.Key);
        }
    }
}
