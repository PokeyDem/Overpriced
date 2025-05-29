using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCDesiredItems : MonoBehaviour
{
    [SerializeField] protected ItemsDatabaseSO _itemsDatabase;
    [SerializeField] protected List<ItemData> _desiredItems;
    private void Awake()
    {
        _desiredItems = new List<ItemData>();
        _desiredItems = SelectDesiredItems();
    }
    public abstract List<ItemData> SelectDesiredItems();
    public List<ItemData> GetDesiredItems() { return _desiredItems; }
}