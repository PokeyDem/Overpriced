using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HagglingUIView : MonoBehaviour
{
    [SerializeField] private GameObject _hagglingUI;

    [SerializeField] private TextMeshProUGUI _itemDescField;
    [SerializeField] private TextMeshProUGUI _resulField;
    [SerializeField] private TextMeshProUGUI _procentField;
    [SerializeField] private TextMeshProUGUI _attemptsLeftField;
    [SerializeField] private TextMeshProUGUI _npcOffer;

    [SerializeField] private Slider _amountSlider;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _acceptOfferButton;
    [SerializeField] private Button _denyOfferButton;

    [SerializeField] private GameObject _haggleWindow;
    [SerializeField] private GameObject _acceptDenyWindow;


    private void Start()
    {

    }
    public void SetActiveHagglingUI(bool isActive)
    {
        _hagglingUI.SetActive(isActive);
    }
    public void UpdatePriceField(int price)
    {
        _inputField.text = price.ToString();
    }
    public void UpdateItemInfo(ItemData item)
    {
        _itemDescField.text = new string(" "+item.Name + "\n\n" + item.Description);
        UpdatePriceField(item.FinalPrice);
        _resulField.text = "";
    }
    public void UpdateResultField(string text, Color color)
    {
        _resulField.color = color;
        _resulField.text = text;
    }
    public void UpdateSlider(int min, int max)
    {
        _amountSlider.minValue = min;
        _amountSlider.maxValue = max;
    }
    public void UpdateSliderValue(int value)
    {
        _amountSlider.value = value;
    }
    public int GetSliderValue()
    {
        return (int) _amountSlider.value;
    }
    public int GetInputValue(int currentValue)
    {
        int price;
        if (_inputField.text.Equals(""))
        {
            return 0;
        }
        if (int.TryParse(_inputField.text, out price))
        {
            return price;
        }
        return currentValue;
    }
    public void UpdateProcentField(int baseValue,int currentValue)
    {
        _procentField.text = (((double)currentValue / baseValue) * 100).ToString("0")+"%";
    }
    public void UpdateAttemptsLeftField(int attempts)
    {
        _attemptsLeftField.text=attempts.ToString()+"/3";
    }
    public void UpdateUIInteraction(bool isActive)
    {
        _sellButton.interactable = isActive;
        _amountSlider.interactable=isActive;
        _inputField.interactable=isActive;
        _acceptOfferButton.interactable=isActive;
        _denyOfferButton.interactable=isActive;
    }
    public void TogglePriceInteraction(bool isActive)
    {
        _amountSlider.interactable = isActive;
        _inputField.interactable = isActive;
    }
    public void UpdateNpcOffer(int value)
    {
        _npcOffer.text= value.ToString();
    }
    public void ToggleAcceptDenyOfferWindow(bool isActive)
    {
        _acceptDenyWindow.SetActive(isActive);
    }
    public void ToggleHaggleWindow(bool isActive)
    {
        _haggleWindow.SetActive(isActive);
    }
}
