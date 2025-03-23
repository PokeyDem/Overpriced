using System;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace ManagerScripts {
    public class MerchantGuildManager : MonoBehaviour {
        [SerializeField] private MerchantGuildItemPoolSo itemPool;
        [SerializeField] private GameObject itemInfoPanel;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private GameObject itemGrid;
        [SerializeField] private TextMeshProUGUI buyCommunicat;
        public UnityEvent<int> buyItemEvent;
        public UnityEvent<int> spendMoneyEvent;
        private List<GameObject> _shopPositions;
        private ItemSlotUIController _selectedItemSlot;

        public void Awake() {
            _shopPositions = new List<GameObject>();
            buyCommunicat.text = "";
        }

        private void Start() {
            AddShopPositions();
        }

        private void AddShopPositions() {
            var itemList = itemPool.GetItemsData();
            //Debug.Log(itemList.Count);
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
            //On inventory slot button click

            if (_selectedItemSlot)
                _selectedItemSlot.DisableOutline();

            _selectedItemSlot = _shopPositions[selectedSlotId].GetComponent<ItemSlotUIController>();
            _selectedItemSlot.EnableOutline();

            int currentItemId = _selectedItemSlot.GetItem().ID;
            if (currentItemId != -1) {
                itemInfoPanel.SetActive(true);
                itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(currentItemId);
            }
            else {
                itemInfoPanel.SetActive(false);
            }
        }

        public string GetItemInfo(int index) {
            ItemData currentItemData = itemPool.database._itemsData.Find(data => data.ID == index);
            return "Name: " + currentItemData.Name
                            + "\nPrice: " + currentItemData.FinalPrice
                            + "\nRarity: " + new string(Convert.ToChar("*"), currentItemData.Rarity)
                            + "\nDescription: " + currentItemData.Description;
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
                buyCommunicat.text = "Not chosen item";
                return;
            }
            if (_selectedItemSlot.GetItemQuantity() <= 0) {
                buyCommunicat.text = "Don't enough stock";
                return;
            }
            if (_selectedItemSlot.GetItem().FinalPrice <= MoneyManager.MoneyManagerInstance.GetCurrentMoney()) {
                buyItemEvent.Invoke(_selectedItemSlot.GetItem().ID);
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


    }
}