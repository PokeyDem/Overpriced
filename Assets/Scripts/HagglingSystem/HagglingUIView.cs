using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class HagglingUIView : MonoBehaviour
{
    [SerializeField] private GameObject _hagglingUI;
    [SerializeField] TextMeshProUGUI _counterField;
    [SerializeField] TextMeshProUGUI _itemDescField;
    [SerializeField] TextMeshProUGUI _resulField;

    public void SetActiveHagglingUI(bool isActive)
    {
        _hagglingUI.SetActive(isActive);
    }
    public void UpdatePriceField(int price)
    {
        _counterField.text = price.ToString();
    }
    public void UpdateItemInfo(ItemData item)
    {
        _itemDescField.text = new string("Item name:\n" + item.Name + "\nDescription:\n" + item.Description + "\nBase price:\n" + item.FinalPrice);
        UpdatePriceField(item.FinalPrice);
        _resulField.text = "";
    }
    public void UpdateResultField(string text, Color color)
    {
        _resulField.color = color;
        _resulField.text = text;
    }
}
