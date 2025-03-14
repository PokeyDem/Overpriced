using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AristocratItemSelector : MonoBehaviour, IItemSelector
{
    [SerializeField] ItemsDatabaseSO _itemsDatabase;
    public List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = _itemsDatabase._itemsData.FindAll(x => x.FinalPrice <= 50);

        return itemsToBuy;
    }
}
