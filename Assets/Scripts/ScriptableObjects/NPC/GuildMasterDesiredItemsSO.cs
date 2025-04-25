using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuildMasterDesiredItemsSO", menuName = "ScriptableObjects/NPCDesiredItemsSO/GuildMaster")]
public class GuildMasterDesiredItemsSO : NPCDesiredItemsSO
{

    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = GetItemsDatabase().FindAll(x => x.ItemType == ItemType.Weapon||x.ItemType==ItemType.Armor);
        return itemsToBuy;
    }


}
