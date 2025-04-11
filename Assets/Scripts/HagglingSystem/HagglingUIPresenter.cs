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
        _view.UpdateItemInfo(_model.getItem());
        _view.SetActiveHagglingUI(true);
    }

    private void OnPriceChanged() =>
        _view.UpdatePriceField(_model.getCurrentPrice());
    private void OnItemSold() =>
        _view.UpdateResultField("Sold",Color.green);
    private void OnItemDenied() =>
        _view.UpdateResultField("Failed", Color.red);

    private void OnHagglingEnded() =>
        _view.SetActiveHagglingUI(false);
}
