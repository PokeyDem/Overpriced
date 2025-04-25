using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AristocratDesiredItemsSO", menuName = "ScriptableObjects/NPCDesiredItemsSO/Aristocrat")]
public class AristocratDesiredItemsSO : NPCDesiredItemsSO
{

    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = GetItemsDatabase().FindAll(x => x.FinalPrice>100);
        return itemsToBuy;
    }


}