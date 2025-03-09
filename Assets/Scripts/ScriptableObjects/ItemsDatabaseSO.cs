using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemsDatabaseSO : ScriptableObject{
    public List<ItemData> _itemsData;
}

[Serializable]
public class ItemData{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Sprite PreviewImage { get; private set; }
    [field: SerializeField] public int BasePrice { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField, Range(0,2)] public int Rarity{ get; private set; }

    public int FinalPrice =>  BasePrice + (int)(BasePrice / 100.0 * 20 * Rarity);
}
