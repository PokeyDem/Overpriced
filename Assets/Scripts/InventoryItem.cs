using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour{
    private ItemData _item;
    private Image _image;

    public void InitializeItem(ItemData item){
        _item = item;
        _image.sprite = item.PreviewImage;
    }
}
