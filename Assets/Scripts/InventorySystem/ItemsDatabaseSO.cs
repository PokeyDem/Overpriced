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
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
}
