using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class NPCDesiredItemsSO : ScriptableObject
{
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private List<ItemData> _desiredItems;
#if UNITY_EDITOR
    private void OnEnable()
    {
        _desiredItems = new List<ItemData>();
        _desiredItems = SelectDesiredItems();
    }
#endif

    public abstract List<ItemData> SelectDesiredItems();
    public List<ItemData> GetDesiredItems() { return _desiredItems; }
    public ItemsDatabaseSO GetItemsDatabase() { return _itemsDatabase; }
}

