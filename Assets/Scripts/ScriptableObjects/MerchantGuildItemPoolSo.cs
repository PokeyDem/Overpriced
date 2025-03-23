using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "MerchantGuildItemPool", menuName = "MerchantGuildItemPool", order = 0)]
    public class MerchantGuildItemPoolSo : ScriptableObject {
        public ItemsDatabaseSO database;
        public List<ItemPoolPosition> itemsPool = new();

        public List<ItemData> GetItemsData() {
            List<ItemData> itemDataList = new List<ItemData>();
            for (int i = 0; i < itemsPool.Count; i++) {
                if (itemsPool[i].IsNotGuaranteed) {
                    if (Random.Range(0f, 1f) <= itemsPool[i].Probability) {
                        continue;
                    }
                }
                var item = database._itemsData.Find(data => data.ID == itemsPool[i].Id);
                AddItemData(itemDataList,item, itemsPool[i].Amount);
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
    public class ItemPoolPosition {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public bool IsNotGuaranteed { get; private set; }
        [field: SerializeField] public float Probability { get; private set; }
        
    }
}