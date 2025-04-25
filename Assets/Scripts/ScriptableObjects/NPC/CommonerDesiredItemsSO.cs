using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommonerDesiredItemsSO", menuName = "ScriptableObjects/NPCDesiredItemsSO/Commoner")]
public class CommonerDesiredItemsSO : NPCDesiredItemsSO
{

    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = GetItemsDatabase().FindAll(x => x.FinalPrice<100);
        return itemsToBuy;
    }


}

