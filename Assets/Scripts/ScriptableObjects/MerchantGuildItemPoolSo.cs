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
            /*if (day <= 0) {
                day = 1;
            }*/
            day--;
            day = day%itemsPool.Count;
            List<ItemData> itemDataList = new List<ItemData>();
            
            for (int i = 0; i < itemsPool[day].DayItemPool.Count; i++) {
                if (itemsPool[day].DayItemPool[i].IsNotGuaranteed) {
                    if (Random.Range(0, 100) <= itemsPool[day].DayItemPool[i].Probability) {
                        continue;
                    }
                }
                var item = database._itemsData.Find(data => data.ID == itemsPool[day].DayItemPool[i].Id);
                if (item.Rarity<=ExperienceManager.ExperienceManagerInstance.GetLevel()) {
                    AddItemData(itemDataList,item, itemsPool[day].DayItemPool[i].Amount);
                }
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
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public bool IsNotGuaranteed { get; private set; }
        [field: SerializeField] public int Probability { get; private set; }
        
    }
}