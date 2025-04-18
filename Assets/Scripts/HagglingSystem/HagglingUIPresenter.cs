using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HagglingUIPresenter : MonoBehaviour
{
    [SerializeField] private HagglingManager _model;
    [SerializeField] private HagglingUIView _view;

    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _acceptOfferButton;
    [SerializeField] private Button _denyOfferButton;

    private void Awake()
    {
        _model.HagglingInitiated += OnHagglingInitiated;
        _model.PriceChanged += OnPriceChanged;
        _model.ItemSold += OnItemSold;
        _model.ItemDenied += OnItemDenied;
        _model.HagglingEnded += OnHagglingEnded;
        _model.NrOfAttemptsChanged += OnNrOfAttemptsChanged;
        _model.UIDisabled += OnUIDisabled;
        _model.AttemptsDepleted += OnAttemptsDepleted;

        _acceptOfferButton.onClick.AddListener(_model.SellItem);
        _denyOfferButton.onClick.AddListener(_model.DenySell);
    }

    private void OnHagglingInitiated()
    {
        _view.UpdateItemInfo(_model.GetItem());
        _view.UpdateSlider(0, _model.GetBasePrice() * _model.MaxPriceMultiplier);
        _view.UpdateSliderValue(_model.GetCurrentPrice());
        _view.UpdatePriceField(_model.GetCurrentPrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetCurrentPrice());
        _view.UpdateAttemptsLeftField(_model.GetNrOfAttemptsLeft());
        _view.UpdateNpcOffer(_model.GetNpcOffer());
        _view.ToggleHaggleWindow(true);
        _view.ToggleAcceptDenyOfferWindow(false);
        _view.UpdateUIInteraction(true);
        _view.SetActiveHagglingUI(true);
    }

    private void OnPriceChanged()
    {
        _view.UpdatePriceField(_model.GetCurrentPrice());
        _view.UpdateSliderValue(_model.GetCurrentPrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetCurrentPrice());
    }
    private void OnItemSold() =>
        _view.UpdateResultField("A fair exchange, I’d say.", Color.green);
    private void OnItemDenied() 
    {
        _view.UpdateResultField("This is a waste of time. We’re done here.", Color.red);
        _view.UpdateAttemptsLeftField(_model.GetNrOfAttemptsLeft());
    }

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
    private void OnNrOfAttemptsChanged()
    {
        _view.UpdateAttemptsLeftField(_model.GetNrOfAttemptsLeft());
        _view.UpdateNpcOffer(_model.GetNpcOffer());
        UpdateHint();
        _view.UpdateSlider(_model.GetNpcOffer(), _model.GetCurrentPrice());
    }
    private void OnUIDisabled()
    {
        _view.UpdateUIInteraction(false);
    }

    private void UpdateHint()
    {
        int basePrice=_model.GetBasePrice();
        int currentPrice=_model.GetCurrentPrice();
        float maxAcceptableMarkup=_model.GetMaxAcceptableMarkup();
        float percentageIncreaseDecimal = (float)currentPrice / basePrice - 1f;
        float difference = percentageIncreaseDecimal - maxAcceptableMarkup;
        if (difference > maxAcceptableMarkup/2)
        {
            _view.UpdateResultField("That price is outrageous!", Color.yellow);
        }
        else if (difference > maxAcceptableMarkup / 3)
        {
            _view.UpdateResultField("I won’t pay that much.", Color.yellow);
        }
        else if (difference > 0)
        {
            _view.UpdateResultField("It’s slightly over my limit.", Color.yellow);
        }

    }
    private void OnAttemptsDepleted()
    {
        _view.ToggleHaggleWindow(false);
        _view.ToggleAcceptDenyOfferWindow(true);
        _view.TogglePriceInteraction(false);
        _view.UpdateResultField("I've lost my patience. This is my last offer. Take it or stop wasting my time.", Color.yellow);
    }
}
