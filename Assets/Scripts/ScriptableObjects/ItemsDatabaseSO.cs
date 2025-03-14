using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemsDatabaseSO : ScriptableObject{
    public List<ItemData> _itemsData = new();

    private void OnEnable(){
        List<ItemData> baseItems = new List<ItemData>();
        foreach (ItemData item in _itemsData){
            if (!item.Name.Contains("*")){
                baseItems.Add(new ItemData(
                    item.Name, item.ItemType, 
                    item.ID, item.Prefab, 
                    item.PreviewImage, 
                    item.BasePrice, item.Description));
            }
        }
        _itemsData.Clear();
        int id = baseItems.Count - 1;
        
        foreach (var item in baseItems){
            _itemsData.Add(item);
            for (int rarity = 1; rarity <= 5; rarity++){
                _itemsData.Add(
                    new ItemData(item, rarity, id++));
            }
        }
    }
}

[Serializable]
public class ItemData{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public ItemType ItemType { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Sprite PreviewImage { get; private set; }
    [field: SerializeField] public int BasePrice { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    public int Rarity{ get; private set; }
    public int FinalPrice =>  BasePrice + (int)(BasePrice / 100.0 * 20 * Rarity); //TODO move price increase percentage to config

    public ItemData(ItemData baseItem, int rarity, int id){
        Name = baseItem.Name + "(" + new String(Convert.ToChar("*"), rarity) + ")";
        ID = id;
        ItemType = baseItem.ItemType;
        Prefab = baseItem.Prefab;
        PreviewImage = baseItem.PreviewImage;
        BasePrice = baseItem.BasePrice;
        Description = baseItem.Description;
        Rarity = rarity;
    }

    public ItemData(string name, ItemType itemType, int id, GameObject prefab, Sprite previewImage, int basePrice, string description){
        Name = name;
        ItemType = itemType;
        ID = id;
        Prefab = prefab;
        PreviewImage = previewImage;
        BasePrice = basePrice;
        Description = description;
    }
}

public enum ItemType{
    Weapon,
    Food,
    Potion
}
