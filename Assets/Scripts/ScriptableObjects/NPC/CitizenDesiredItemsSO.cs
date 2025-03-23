using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CitizenDesiredItemsSO", menuName = "ScriptableObjects/NPCDesiredItemsSO/Citizen")]
public class CitizenDesiredItemsSO : NPCDesiredItemsSO
{

    public override List<ItemData> SelectDesiredItems()
    {
        List<ItemData> itemsToBuy = GetItemsDatabase()._itemsData.FindAll(x => x.ItemType!=ItemType.Weapon);
        return itemsToBuy;
    }


}

