using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class HagglingManager : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isPlayerReady;
    [SerializeField] private bool _isNpcReady;
    private NpcBehaviour _npcBehaviour;
    private int _currentPrice;
    private int _basePrice;
    [SerializeField] private int _nrOfChancesLeft=3;
    [SerializeField] private bool _hagglingInProgress = false;
    [SerializeField] private int _npcOffer = 0;

    public event Action HagglingInitiated;
    public event Action HagglingEnded;
    public event Action PriceChanged;
    public event Action NrOfChancesChanged;
    public event Action UIDisabled;

    public event Action ItemSold;
    public UnityEvent<float> onItemSold;
    public event Action ItemDenied;

    private float _toleranceDecimal;
    private float _randomDeviation;
    private float _maxThreshold;
    private float _successChance;


    public readonly int MaxPriceMultiplier = 3;


    public void SetPlayerReadiness(bool isPlayerReady){
        _isPlayerReady = isPlayerReady;
    }

    public void SetNpcReadiness(bool isNpcReady){
        _isNpcReady = isNpcReady;
    }

    private void CheckReadiness(){
        if (_isNpcReady&&!_hagglingInProgress)
        {
            StartHaggling();
        }
    }

    public void SetNpc(NpcBehaviour npcBehaviour){
        _npcBehaviour = npcBehaviour;
    }

    public void IncreaseCounter(){
        if (_currentPrice < _basePrice * 2){
            _currentPrice += 10;
            PriceChanged?.Invoke();
        }
    }

    public void DecreaseCounter(){
        if (_currentPrice > 10){
            _currentPrice -= 10;
            PriceChanged?.Invoke();
        }
    }

    public void StartHaggling(){
        if(_npcBehaviour==null || _npcBehaviour.GetDisplaySlotController().GetItem()==null)
        {
            _npcBehaviour.GoToExitWithoutItem();
            return;
        }
        _hagglingInProgress = true;
        _nrOfChancesLeft = 3;
        _basePrice = _npcBehaviour.GetItemToBuy().FinalPrice;
        _currentPrice = _basePrice;
        _npcOffer = _basePrice;

        _toleranceDecimal = _npcBehaviour.GetToleranceDecimal();
        _randomDeviation = Random.Range(350, 480);
        _randomDeviation = _randomDeviation / 100;
        _maxThreshold = (int)(_basePrice + _basePrice * _toleranceDecimal * _randomDeviation);
        float random = Random.value;
        //float bias = random * random;
        _successChance = Mathf.Lerp(_toleranceDecimal, _toleranceDecimal * 2.5f, random);

        HagglingInitiated?.Invoke();
    }

    public void TryToSell(){
        int markupPoints = (int)Mathf.Floor(((float)_currentPrice / _basePrice * 100 - 100) / 10);
        float percentageIncreaseDecimal = (float)_currentPrice / _basePrice - 1f;


        if (percentageIncreaseDecimal <= _toleranceDecimal)
        {
            SellItem();
        }
        else
        {
            Debug.Log("chance: "+_successChance+", increase: "+percentageIncreaseDecimal);
            if (percentageIncreaseDecimal <= _successChance)
            {
                SellItem();
            }
            else if (_nrOfChancesLeft>1 && _currentPrice<= _maxThreshold)
            {
                _nrOfChancesLeft--;
                _npcOffer = (int)(_basePrice + Mathf.Round(_basePrice* 0.05f*(3-_nrOfChancesLeft)));
                NrOfChancesChanged?.Invoke();
            }
            else{
                DenySell();
            }
        }
        
    }

    private void SellItem(){
        MoneyManager.MoneyManagerInstance.PutMoney(_currentPrice);
        ItemSold?.Invoke();
        onItemSold?.Invoke(_currentPrice*_toleranceDecimal);
        _npcBehaviour.GetDisplaySlotController().RemoveItem();
        StartCoroutine(EndHaggling(true));
    }

    private void DenySell(){
        ItemDenied?.Invoke();
        StartCoroutine(EndHaggling(false));
    }

    private IEnumerator EndHaggling(bool itemSold){
        _nrOfChancesLeft=0;
        NrOfChancesChanged?.Invoke();
        UIDisabled?.Invoke();
        yield return new WaitForSeconds(1);
        _npcBehaviour.GoToExit(itemSold);
        HagglingEnded?.Invoke();
        _hagglingInProgress = false;
        // StartCoroutine(DisableUiDelay());
    }
   

    public void Interact()
    {
        CheckReadiness();
    }

    public string TriggerInteractPrompt()
    {
        return "Start Haggling";
    }

    public int GetCurrentPrice()
    {
        return _currentPrice;
    }
    public int GetBasePrice()
    {
        return _basePrice;
    }
    public ItemData GetItem()
    {
        return _npcBehaviour.GetItemToBuy();
    }
    public void SetCurrentPrice(int price)
    {
        _currentPrice = price;
    }
    public NpcBehaviour GetNpc()
    {
        return _npcBehaviour;
    }
    public int GetNrOfChancesLeft()
    {
        return _nrOfChancesLeft;
    }
    public int GetNpcOffer()
    {
        return _npcOffer;
    }
}
