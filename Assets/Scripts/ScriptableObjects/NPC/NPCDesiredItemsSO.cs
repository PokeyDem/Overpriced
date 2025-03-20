using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCDesiredItemsSO : ScriptableObject
{
    [SerializeField] private ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private List<ItemData> _desiredItems = new List<ItemData>();
    private void OnEnable()
    {
        _desiredItems.Clear();
        _desiredItems = SelectDesiredItems();
    }

    public abstract List<ItemData> SelectDesiredItems();
    public List<ItemData> GetDesiredItems() { return _desiredItems; }
    public ItemsDatabaseSO GetItemsDatabase() { return _itemsDatabase; }
}

