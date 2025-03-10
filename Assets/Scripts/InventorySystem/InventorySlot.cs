using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour{
    private int _itemId = -1;
    private Image _image;
    private TextMeshProUGUI _quantityDisplay;
    private Image _outline;
    private int _quantity;
    private bool _isOutlineEnabled;

    private void Awake(){
        _image = gameObject.transform.Find("ItemImage").GetComponent<Image>();
        _outline = gameObject.transform.Find("Outline").GetComponent<Image>();
        _quantityDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AddItem(int itemId, Sprite image){
        _itemId = itemId;
        _image.sprite = image;
        _quantity = 1;
        _quantityDisplay.text = _quantity.ToString();
    }

    public void EnableOutline(){
        Color color = _outline.color;
        color.a = 1f; 
        _outline.color = color;
    }

    public void DisableOutline(){
        Color color = _outline.color;
        color.a = 0f; 
        _outline.color = color;
    }

    public void RemoveItem(){
        _itemId = -1;
        _image.sprite = null;
        _quantityDisplay.text = "";
    }

    public int GetItemId(){
        return _itemId;
    }

    public Image GetImage(){
        return _image;
    }

    public int GetItemQuantity(){
        return _quantity;
    }

    public void IncreaseQuantity(){
        _quantity++;
        _quantityDisplay.text = _quantity.ToString();
    }

    public void DecreaseQuantity(){
        _quantity--;
        _quantityDisplay.text = _quantity.ToString();
    }

    public bool IsEmpty(){
        return _itemId == -1;
    }
}
