using DefaultNamespace;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace ManagerScripts {
    public class MerchantGuildManager : MonoBehaviour {
        [SerializeField] private MerchantGuildItemPoolSo itemPool;
        [SerializeField] private GameObject itemInfoPanel;
        [SerializeField] private Image _itemIconImage;
        [SerializeField] private TextMeshProUGUI _itemNameField;
        [SerializeField] private TextMeshProUGUI _itemDescField;
        [SerializeField] private TextMeshProUGUI _itemPriceField;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private TextMeshProUGUI buyCommunicat;
        public UnityEvent<ItemData> buyItemEvent;
        public UnityEvent buyItemEventTutorial;
        public UnityEvent<int> spendMoneyEvent;
        private List<GameObject> _shopPositions;
        private ItemSlotUIController _selectedItemSlot;

        public void Awake() {
            _shopPositions = new List<GameObject>();
            buyCommunicat.text = "";
        }

        private void AddShopPositions(int day){
            var itemList = itemPool.GetItemsData(day);
            
            for (int i = 0; i < itemList.Count; i++) {
                
                int index = FindExistingItem(itemList[i]);
                
                if (index != -1) {
                    _shopPositions[index].GetComponent<ItemSlotUIController>().AddItem(itemList[i]);
                    continue;
                }

                GameObject slot = Instantiate(itemSlotPrefab, itemGrid.transform);
                _shopPositions.Add(slot);
                int slotIndex = _shopPositions.Count - 1;
                slot.GetComponent<ItemSlotUIController>().AddItem(itemList[i]);
                slot.GetComponentInChildren<Button>().onClick.AddListener(() => SelectSlot(slotIndex));
            }

        }

        public void SelectSlot(int selectedSlotId) {

            if (_selectedItemSlot)
                _selectedItemSlot.DisableOutline();

            _selectedItemSlot = _shopPositions[selectedSlotId].GetComponent<ItemSlotUIController>();
            _selectedItemSlot.EnableOutline();

            ItemData currentItem = _selectedItemSlot.GetItem();
            
            if (currentItem != null) {
                _itemIconImage.sprite = currentItem.PreviewImage;
                _itemNameField.text = currentItem.Name;
                _itemDescField.text = currentItem.Description;
                _itemPriceField.text = "Price: " + currentItem.FinalPrice;
                itemInfoPanel.SetActive(true);
                //itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(currentItem);
            }
            else {
                itemInfoPanel.SetActive(false);
            }
        }

        public string GetItemInfo(ItemData index) {
            return "Name: " + index.Name
                            + "\nPrice: " + index.FinalPrice
                            + "\nRarity: " + new string(Convert.ToChar("*"), index.Rarity)
                            + "\nDescription: " + index.Description;
        }

        private int FindExistingItem(ItemData item) {
            int counter = 0;
            foreach (var itemSlot in _shopPositions) {
                if (itemSlot.GetComponent<ItemSlotUIController>().GetItem().ID == item.ID) {
                    return counter;
                }

                counter++;
            }

            return -1;
        }

        public void TryBuyItem() {
            if (_selectedItemSlot==null) {
                buyCommunicat.text = "No item selected";
                return;
            }
            if (_selectedItemSlot.GetItemQuantity() <= 0) {
                buyCommunicat.text = "Out of stock";
                return;
            }
            if (_selectedItemSlot.GetItem().FinalPrice <= MoneyManager.Instance.GetCurrentMoney()) {
                buyItemEvent.Invoke(_selectedItemSlot.GetItem());
                buyItemEventTutorial?.Invoke();
                spendMoneyEvent.Invoke(_selectedItemSlot.GetItem().FinalPrice);
                _selectedItemSlot.DecreaseQuantity();
            }else {
                if (Random.Range(1, 1000) == 1) {
                    buyCommunicat.text = "How sad you are too poor to buy this";
                }else {
                    buyCommunicat.text = "Not enough money";
                }
            }
        }

        public void RestockOffer(int day){
            while(0 < _shopPositions.Count) {
                var tmp = _shopPositions[0];
                _shopPositions.Remove(tmp);
                Destroy(tmp);
            }
            AddShopPositions(day);
        }
    }
}