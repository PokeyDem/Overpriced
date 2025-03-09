using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCitizen : NpcBehaviour
{
    [SerializeField] ItemsDatabaseSO _itemsDatabase;
    public override ItemData ChooseItem()
    {
        ItemData item = null;
        List<ItemData> itemsToBuy = _itemsDatabase._itemsData.FindAll(x => x.FinalPrice > 50 && x.FinalPrice<100);
        if (itemsToBuy.Count > 0)
        {
            item = itemsToBuy[Random.Range(0, itemsToBuy.Count)];
        }
        return item;
    }

    public override string GetNpcType()
    {
        return "Citizen";
    }
}
