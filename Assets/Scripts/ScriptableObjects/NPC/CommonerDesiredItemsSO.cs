using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommonerDesiredItemsSO", menuName = "ScriptableObjects/NPCDesiredItemsSO/Commoner")]
public class CommonerDesiredItemsSO : NPCDesiredItemsSO
{

    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = GetItemsDatabase()._itemsData.FindAll(x => x.Rarity<3);
        return itemsToBuy;
    }


}

