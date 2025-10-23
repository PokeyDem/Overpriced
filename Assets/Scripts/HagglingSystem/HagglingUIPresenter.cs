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
    [SerializeField] private Sprite _emoteHappy;
    [SerializeField] private Sprite _emoteSad;
    [SerializeField] private Sprite _emoteAngry; 
    [SerializeField] private Sprite _emoteAnnoyed;
    [SerializeField] private Sprite _emoteThinking;
    [SerializeField] private Sprite _emoteAngrier;

    private void Awake()
    {
        _model.HagglingInitiated.AddListener(OnHagglingInitiated);
        _model.PriceChanged.AddListener(OnPriceChanged);
        _model.ItemSold.AddListener(OnItemSold);
        _model.ItemDenied.AddListener(OnItemDenied);
        _model.HagglingEnded.AddListener(OnHagglingEnded);
        _model.NrOfAttemptsChanged.AddListener(OnNrOfAttemptsChanged);
        _model.UIDisabled.AddListener(OnUIDisabled);
        _model.AttemptsDepleted.AddListener(OnAttemptsDepleted);
        _acceptOfferButton.onClick.AddListener(_model.SellItem);
        _denyOfferButton.onClick.AddListener(_model.DenySell);
    }

    private void OnHagglingInitiated()
    {
        _view.SetEmote(_emoteThinking);
        _view.UpdateItemInfo(_model.GetItem());
        _view.UpdateSlider(0, _model.GetBasePrice() * _model.MaxPriceMultiplier);
        _view.UpdateSliderValue(_model.GetBasePrice());
        _view.UpdatePriceField(_model.GetBasePrice());
        _view.UpdateProcentField(_model.GetBasePrice(), _model.GetBasePrice());
        _view.UpdateAttemptsLeftField(_model.GetNrOfAttemptsLeft());
        _view.UpdateNpcOffer(_model.GetNpcOffer());
        _view.UpdateResultField($"Hi, I'd like to buy this {_model.GetItem().Info.Name}.");
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
    private void OnItemSold()
    {
        _view.SetEmote(_emoteHappy);
        _view.UpdateResultField("A fair exchange, I’d say.");
    }
    private void OnItemDenied() 
    {
        _view.SetEmote(_emoteAngrier);
        _view.UpdateResultField("This is a waste of time. We’re done here.");
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
            _view.UpdateResultField("That price is outrageous!");
            _view.SetEmote(_emoteAngrier);
        }
        else if (difference > maxAcceptableMarkup / 3)
        {
            _view.UpdateResultField("I won’t pay that much.");
            _view.SetEmote(_emoteAngry);
        }
        else if (difference > 0)
        {
            _view.UpdateResultField("It’s slightly over my limit.");
            _view.SetEmote(_emoteAnnoyed);
        }

    }
    private void OnAttemptsDepleted()
    {
        _view.ToggleHaggleWindow(false);
        _view.ToggleAcceptDenyOfferWindow(true);
        _view.TogglePriceInteraction(false);
        _view.UpdateResultField("I've lost my patience. This is my last offer. Decide now and move on.");
        _view.SetEmote(_emoteAngry);
    }
}
