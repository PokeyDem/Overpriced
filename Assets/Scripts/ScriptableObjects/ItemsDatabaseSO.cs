using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class ItemsDatabaseSO : ScriptableObject{
    public List<ItemFamily> _itemsDataFamilys;

    public ItemData GetRandomItemData(ItemType itemType, int rarity) {
        List<ItemData> tmp = new List<ItemData>();
        foreach (var itemFamily in _itemsDataFamilys) {
            if (itemFamily.Info.ItemType == itemType) {
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
    
    private void OnEnable() {
        for (int i=0; i<_itemsDataFamilys.Count; i++) {
            _itemsDataFamilys[i].Init(i);
        }
    }

    public void ImplementEvent(RandomEvent randomEvent) {
        foreach (var itemFamily in _itemsDataFamilys) {
            if (itemFamily.Info.ItemType.Equals(randomEvent.itemType)) {
                itemFamily.Info.priceModifier = randomEvent.modifier;
            }else {
                itemFamily.Info.priceModifier =  1;
            }
        }
    }

    public void ClearEvents(){
        foreach (var itemFamily in _itemsDataFamilys) {
            itemFamily.Info.priceModifier =  1;
        }
    }
}
[Serializable]
public class BaseItemInfo
{
    public int ID { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public int BasePrice { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    public float priceModifier;
    public void Init(int groupID) {
        ID= groupID;
        priceModifier = 1;
    }
}
[Serializable]
public class ItemFamily{
    [SerializeField] private BaseItemInfo _info;
    public BaseItemInfo Info => _info;
    [SerializeField] public ItemData[] ItemsVariants;

    public void Init(int groupID) {
        Info.Init(groupID);
        for (int i = 0; i < 6; i++) {
            ItemsVariants[i].Init(Info, i);
        }
    }
    
}

[Serializable]
public class ItemData
{
    [SerializeField] public BaseItemInfo _info;
    public BaseItemInfo Info => _info;
    [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite PreviewImage { get; private set; }
        public int Rarity{ get; private set; }
        public int BasePrice => Info.BasePrice;
        public float PriceModifier => Info.priceModifier;
        public ItemType ItemType => Info.ItemType;
        public string Name {
            get {
                string tmp = Info.Name;
                for (int i = 0; i < Rarity; i++) {
                    tmp += "*";
                }
                return tmp;
            }
        }

        public string Description => Info.Description;
        public int ID => Info.ID * 10 + Rarity;
        public int FinalPrice =>  (int)((BasePrice + ((BasePrice * 20 / 100.0) * (Rarity+((1/4)*math.pow(Rarity,2))))) * PriceModifier);//TODO move price increase percentage to config

        public void Init(BaseItemInfo info, int rarity) {
            
            _info = info;
            Rarity = rarity;
        }
}

public enum ItemType{
    All,
    Weapon,
    Food,
    Potion,
    Armor,
    Medicine
}
