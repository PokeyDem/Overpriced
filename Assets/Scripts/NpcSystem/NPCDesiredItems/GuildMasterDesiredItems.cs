using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildMasterDesiredItems : NPCDesiredItems
{
    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = _itemsDatabase.FindAll(x => x.ItemType == ItemType.Weapon || x.ItemType == ItemType.Armor);
        return itemsToBuy;
    }
}
