using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HagglingUIPresenter : MonoBehaviour
{
    [SerializeField] private HagglingManager _model;
    [SerializeField] private HagglingUIView _view;
    private void Awake()
    {
        _model.HagglingInitiated += OnHagglingInitiated;
        _model.PriceChanged += OnPriceChanged;
        _model.ItemSold += OnItemSold;
        _model.ItemDenied += OnItemDenied;
        _model.HagglingEnded += OnHagglingEnded;
    }

    private void OnHagglingInitiated()
    {
        _view.UpdateItemInfo(_model.GetItem());
        _view.UpdateSlider(0, _model.GetBasePrice() * _model.MaxPriceMultiplier);
        _view.UpdateSliderValue(_model.GetCurrentPrice());
        _view.UpdatePriceField(_model.GetCurrentPrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetCurrentPrice());
        _view.SetActiveHagglingUI(true);
    }

    private void OnPriceChanged()
    {
        _view.UpdatePriceField(_model.GetCurrentPrice());
        _view.UpdateSliderValue(_model.GetCurrentPrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetCurrentPrice());
    }
    private void OnItemSold() =>
        _view.UpdateResultField("Sold",Color.green);
    private void OnItemDenied() =>
        _view.UpdateResultField("Failed", Color.red);

    private void OnHagglingEnded() =>
        _view.SetActiveHagglingUI(false);

    public void OnSliderValueChanged()
    {
        _model.SetCurrentPrice(_view.GetSliderValue());
        _view.UpdatePriceField(_model.GetCurrentPrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetCurrentPrice());
    }
    public void OnInputValueChanged()
    {
        int value = Mathf.Clamp(_view.GetInputValue(_model.GetCurrentPrice()), 0, _model.GetBasePrice() * _model.MaxPriceMultiplier);
        _view.UpdatePriceField(value);
        _model.SetCurrentPrice(value);
        _view.UpdateSliderValue(_model.GetCurrentPrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetCurrentPrice());
    }
}
