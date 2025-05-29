using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenDesiredItems : NPCDesiredItems
{
    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = _itemsDatabase.FindAll(x => x.FinalPrice > 50);
        return itemsToBuy;
    }
}
