using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : SingletonDontDestroyOnLoad<DataTransfer>
{
    private List<ItemData> _itemsToTransfer = new List<ItemData>();

    private new void Awake(){
        base.Awake();
    }
    public void AddItemToTransferList(ItemData item){
        _itemsToTransfer.Add(item);
    }

    public List<ItemData> GetBoughtItems(){
        return _itemsToTransfer;
    }
}
