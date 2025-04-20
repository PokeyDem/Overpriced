using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class ItemsDatabaseSO : ScriptableObject{
    public List<ItemFamily> _itemsDataFamilys = new();

    public ItemData GetRandomItemData(ItemType itemType, int rarity) {
        List<ItemData> tmp = new List<ItemData>();
        foreach (var itemFamily in _itemsDataFamilys) {
            if (itemFamily.ItemType == itemType) {
                tmp.Add(itemFamily.ItemsVariants[rarity]);
            }
        }

        return tmp[Random.Range(0, tmp.Count)];
    }

    public ItemData Find(Predicate<ItemData> match) {
        foreach (var itemFamily in _itemsDataFamilys) {
            foreach (var itemData in itemFamily.ItemsVariants) {
                if (match.Invoke(itemData)) {
                    return itemData;
                }
            }
        }
        return null;
    }

    public List<ItemData> FindAll(Predicate<ItemData> match) {
        List<ItemData> tmp = new List<ItemData>();
        foreach (var itemFamily in _itemsDataFamilys) {
            foreach (var itemData in itemFamily.ItemsVariants) {
                if (match.Invoke(itemData)) {
                    tmp.Add(itemData);
                }
            }
        }
        return tmp;
    }
    
    private void OnValidate() {
        for (int i=0; i<_itemsDataFamilys.Count; i++) {
            _itemsDataFamilys[i].Init(i);
        }
    }
}

[Serializable]
public class ItemFamily{
    public int ID { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public int BasePrice { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [SerializeField] public ItemData[] ItemsVariants = new ItemData[6];

    public void Init(int groupID) {
        ID = groupID;
        for (int i = 0; i < 6; i++) {
            ItemsVariants[i].Init(this,i);
        }
    }
    
}

[Serializable]
public class ItemData {
        private ItemFamily _parent;
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite PreviewImage { get; private set; }
        public int Rarity{ get; private set; }
        public int BasePrice => _parent.BasePrice;
        public ItemType ItemType => _parent.ItemType;
        public string Name {
            get {
                string tmp = _parent.Name;
                for (int i = 0; i < Rarity; i++) {
                    tmp += "*";
                }
                return tmp;
            }
        }

        public string Description => _parent.Description;
        public int ID => _parent.ID * 10 + Rarity;
        public int FinalPrice =>  BasePrice + (int)(BasePrice / 100.0 * 20 * Rarity);//TODO move price increase percentage to config

        public void Init(ItemFamily parent, int rarity) {
            _parent = parent;
            Rarity = rarity;
        }
}

public enum ItemType{
    All,
    Weapon,
    Food,
    Potion,
    Armor
}
