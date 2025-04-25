using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CitizenDesiredItemsSO", menuName = "ScriptableObjects/NPCDesiredItemsSO/Citizen")]
public class CitizenDesiredItemsSO : NPCDesiredItemsSO
{

    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = GetItemsDatabase().FindAll(x => x.FinalPrice>50);
        return itemsToBuy;
    }


}

