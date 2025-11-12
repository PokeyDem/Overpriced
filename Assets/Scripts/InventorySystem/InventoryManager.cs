using System;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : SingletonDontDestroyOnLoad<InventoryManager>, IInteractable
{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private RectTransform _inventoryPanel;
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private GameObject _itemSlotPrefab;
    [SerializeField] private GameObject _inventorySlotsContainer;
    [SerializeField] private MoneyManager _moneyManager; //todo delete when merchants guild is created
    [SerializeField] private RectTransform _contentPanelTransform;
    [SerializeField] private PlayerControl _playerControl;
    [SerializeField] private TextMeshProUGUI _foldButtonText;

    private List<GameObject> _inventorySlots;
    private Dictionary<int, int> _inventorySlotsDictionary; //<itemId, inventorySlotIndex>
    private Dictionary<ItemType, List<int>> _itemTypesDictionary; 
    
    private DisplaySlotController _currentDisplayDisplaySlot; // todo refactor and delete later
    private ItemSlotUIController _selectedInventorySlot;
    private int _selectedSlotId;

    private ItemType _categoryToSortBy = ItemType.All;
    private ItemType[] _itemTypes; 
    private int _itemTypeIndex = 0;

    private float _speed = 300;
    private bool _foldToggle = true;

    [SerializeField] private TextMeshProUGUI _itemCategoryText; //Text for the sort by category button
    
    private const int DEFAULT_SLOT_COUNT = 15; //Todo move to config later

    private new void Awake(){
        base.Awake();
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
            if (slot.GetComponent<ItemSlotUIController>().GetItem() != null) 
                slot.GetComponent<ItemSlotUIController>().RemoveItem();
        }
        _inventorySlotsDictionary = new Dictionary<int, int>();
        _itemTypesDictionary = new Dictionary<ItemType, List<int>>();
        InitItemTypesDictionary();
    }

    public void SetNearestSlot(DisplaySlotController nearestDisplaySlot){
        _currentDisplayDisplaySlot = nearestDisplaySlot;
        if (_currentDisplayDisplaySlot != null&&_inventoryUI.gameObject.activeSelf)
        {
            _playerControl.SetInteractable(this);
        }
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
        if (_inventoryUI.gameObject.activeSelf)
            return;
        
        AudioManager.PlaySound(SoundType.INTERACT, 0.2f);
        _inventoryUI.gameObject.SetActive(true);
        SelectSlot(0);
        _selectedInventorySlot.EnableOutline();
        _playerControl.SetInteractable(this);
        _playerControl.ScrollUp.AddListener(SelectPreviousSlot);
        _playerControl.ScrollDown.AddListener(SelectNextSlot);
    }

    public void DisableInventory(){
        if ( !_inventoryUI.gameObject.activeSelf)
        {
            return;
        }
        _inventoryUI.gameObject.SetActive(false);
        //if(_playerControl.GetInteractable() == this as IInteractable)
        _playerControl.SetInteractable(null);
        _playerControl.ScrollUp.RemoveListener(SelectPreviousSlot);
        _playerControl.ScrollDown.RemoveListener(SelectNextSlot);
    }
    public void FoldUnfoldInventory()
    {
        if(_foldToggle)
        {
            MoveInventoryTowards(-300f);
            _foldButtonText.transform.Rotate(0, 0, 180);
        }
        else
        {
            MoveInventoryTowards(-425f);
            _foldButtonText.text = "^^^";
            _foldButtonText.transform.Rotate(0, 0, 180);
        }
        SelectSlot(0);
        _foldToggle = !_foldToggle;
    }
    private void MoveInventoryTowards(float target1)
    {
        Vector2 offset = _inventoryPanel.anchoredPosition;
        offset.y =  target1;
        _inventoryPanel.anchoredPosition = offset;
    }

    public void AddItemToDisplaySlot(){ //On AddButton click
        var inventorySlotItem = _selectedInventorySlot.GetItem();
        
        if (inventorySlotItem != null && _currentDisplayDisplaySlot.ItemData == null && _currentDisplayDisplaySlot.IsBought){
            _currentDisplayDisplaySlot.PlaceItem(inventorySlotItem);
            RemoveItemFromInventory();
        }

        if (!_currentDisplayDisplaySlot.IsBought) {
            _currentDisplayDisplaySlot.TryToBuy();
        }
        _playerControl.SetInteractable(this);
    }

    public void RemoveItemFromInventory(){
        if (_selectedInventorySlot.IsEmpty()) {
            return;
        }

        if (_selectedInventorySlot.GetItemQuantity() > 1){
            _selectedInventorySlot.DecreaseQuantity();
        }
        else{
            _inventorySlotsDictionary.Remove(_selectedInventorySlot.GetItemID());
            _itemTypesDictionary[_selectedInventorySlot.RemoveItem().ItemType].Remove(_selectedSlotId);
            if (_categoryToSortBy != ItemType.All)
                _selectedInventorySlot.gameObject.SetActive(false);
        }
    }

    public void Exit(){ //On ExitButton click
        _inventoryUI.gameObject.SetActive(false);
        _playerControl.SetInteractable(null);
    }

    public void SelectSlot(int selectedSlotId){ //On inventory slot button click

        _selectedSlotId = selectedSlotId;
        
        if (_selectedInventorySlot) 
            _selectedInventorySlot.DisableOutline();
        
        _selectedInventorySlot = _inventorySlots[selectedSlotId].GetComponent<ItemSlotUIController>();
        _selectedInventorySlot.EnableOutline();

        ItemData currentItem = _inventorySlots[selectedSlotId].GetComponent<ItemSlotUIController>().GetItem();
        /*if (currentItem != null){
            _itemInfoPanel.SetActive(true);
            _itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(currentItem);
        }
        else{
            _itemInfoPanel.SetActive(false);
        }*/
        _playerControl.SetInteractable(this);
    }
    public void SelectNextSlot()
    {
        if (_selectedSlotId < 4)
        {
            SelectSlot(_selectedSlotId + 1);
        } else
        {
            SelectSlot(0);
        }
    }
    public void SelectPreviousSlot()
    {
        if (_selectedSlotId > 0)
        {
            SelectSlot(_selectedSlotId -1);
        }
        else
        {
            SelectSlot(4);
        }
    }
    public string GetItemInfo(ItemData currentItemData){
        return "Name: " + currentItemData.Name
            + "\nPrice: " + currentItemData.FinalPrice
            + "\nRarity: " + new string(Convert.ToChar("*"), currentItemData.Rarity)
            + "\nDescription: " + currentItemData.Description;
    }
    public void RemoveItemFromDisplaySlot(){
        ItemData item = _currentDisplayDisplaySlot.ItemData;
        
        if (item == null) return;
        
        _currentDisplayDisplaySlot.RemoveItem();
        AddItemToInventory(item);
        _playerControl.SetInteractable(this);
    }

    public void AddItemToInventory(ItemData item){
        int slotIndex = -1;

        if ((slotIndex = FindExistingItem(item)) != -1){
            _inventorySlots[slotIndex].GetComponent<ItemSlotUIController>().IncreaseQuantity();
            _itemTypesDictionary[item.ItemType].Add(slotIndex);
        }
        else if ((slotIndex = FindFreePosition()) != -1){
            _inventorySlots[slotIndex].GetComponent<ItemSlotUIController>().AddItem(item);
            _inventorySlotsDictionary[item.ID] = slotIndex;
            _itemTypesDictionary[item.ItemType].Add(slotIndex);
        }
        else{
            AddSlots(5);
            IncreaseContentPanelSize();
            AddItemToInventory(item);
        }
        
        EnableSlotsForSpecificCategory(_categoryToSortBy);
    }

    private int FindExistingItem(ItemData index){
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots) {
            ItemData itemData = inventorySlot.GetComponent<ItemSlotUIController>().GetItem();
            if (itemData!=null && itemData.ID.Equals(index.ID)){
                return counter;
            }
            counter++;
        }
        return -1;
    }

    private int FindFreePosition(){
        int counter = 0;
        foreach (var inventorySlot in _inventorySlots){
            if (inventorySlot.GetComponent<ItemSlotUIController>().IsEmpty()){
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
            Debug.Log(counter + ": " + inventorySlot.GetComponent<ItemSlotUIController>().GetItem().ID);
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

    public void LoadInventoryData(List<ItemData> inventoryData){
        ClearInventory();
        foreach (var itemData in inventoryData){
            AddItemToInventory(itemData);
        }
    }

    public void Interact()
    {
        var inventorySlotItem = _selectedInventorySlot.GetItem();
        if (!_currentDisplayDisplaySlot.isOccupied && !_currentDisplayDisplaySlot.isChosen)
        {
            if (inventorySlotItem != null && _currentDisplayDisplaySlot.ItemData != null)
            {
                RemoveItemFromDisplaySlot();
            }
            else if (inventorySlotItem == null && _currentDisplayDisplaySlot.ItemData != null)
            {
                RemoveItemFromDisplaySlot();
                return;
            }
            AddItemToDisplaySlot();
        }
    }

    public string TriggerInteractPrompt()
    {
        if (_currentDisplayDisplaySlot != null)
        {
            if (_currentDisplayDisplaySlot.isChosen)
            {
                return $"Cant remove item chosen by an NPC";
            }
            if (_currentDisplayDisplaySlot.isOccupied)
            {
                return $"Cant remove item occupied by an NPC";
            }
        }
        if (_selectedInventorySlot.GetItem() == null) 
        {
            if (!_currentDisplayDisplaySlot.IsBought) {
                return $"Buy for {_currentDisplayDisplaySlot.Prize} gold";
            }
            if(_currentDisplayDisplaySlot.ItemData != null)
            {
                return $"Remove {_currentDisplayDisplaySlot.ItemData.Name} from Display";
            }
            return "No Item Selected";
        }
        return $"Put {_selectedInventorySlot.GetItem().Name} on Display";
    }
}
