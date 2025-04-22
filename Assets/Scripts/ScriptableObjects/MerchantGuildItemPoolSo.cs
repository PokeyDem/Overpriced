using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "MerchantGuildItemPool", menuName = "MerchantGuildItemPool", order = 0)]
    public class MerchantGuildItemPoolSo : ScriptableObject {
        public ItemsDatabaseSO database;
        public List<ItemPoolDay> itemsPool = new();

        public List<ItemData> GetItemsData(int day) {
            day--;
            day = day%itemsPool.Count;
            List<ItemData> itemDataList = new List<ItemData>();
            
            foreach (var itemPoolPosition in itemsPool[day].DayItemPool) {
                if (itemPoolPosition.IsNotGuaranteed) {
                    if (Random.Range(0, 100) <= itemPoolPosition.Probability) {
                        continue;
                    }
                }

                if (itemPoolPosition.rarity > ExperienceManager.ExperienceManagerInstance.GetLevel()) {
                    continue;
                }
                var item = database.GetRandomItemData(itemPoolPosition.ItemType,
                    itemPoolPosition.rarity);
                AddItemData(itemDataList, item, itemPoolPosition.Amount);
            }
            return itemDataList;
        }

        private void AddItemData(List<ItemData> list, ItemData itemData, int amount) {
            for (int i = 0; i < amount; i++) {
                list.Add(itemData);
            }
        }
    }

    [Serializable]
    public class ItemPoolDay {
        [field: SerializeField] public List<ItemPoolPosition> DayItemPool{ get; private set; }
    }
    
    [Serializable]
    public class ItemPoolPosition {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [SerializeField] public int rarity;
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public bool IsNotGuaranteed { get; private set; }
        [field: SerializeField] public int Probability { get; private set; }
        
    }
}