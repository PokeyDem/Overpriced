using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour{
    private int _itemId = -1;
    private Image _image;

    private void Awake(){
        _image = GetComponent<Image>();
    }

    public void AddItem(int itemId, Sprite image){
        _itemId = itemId;
        _image.sprite = image;
    }

    public void RemoveItem(int itemId){
        _itemId = -1;
        _image.sprite = null;
    }

    public int GetItemId(){
        return _itemId;
    }

    public bool IsEmpty(){
        return _itemId == -1;
    }
}
