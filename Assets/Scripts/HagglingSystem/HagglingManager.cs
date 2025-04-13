using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class HagglingManager : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isPlayerReady;
    [SerializeField] private bool _isNpcReady;
    private NpcBehaviour _npcBehaviour;
    private int _currentPrice;
    private int _basePrice;
    [SerializeField] private int _nrOfChancesLeft=2;
    [SerializeField] private bool _hagglingInProgress = false;

    public event Action HagglingInitiated;
    public event Action HagglingEnded;
    public event Action PriceChanged;

    public event Action ItemSold;
    public event Action ItemDenied;


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
        _nrOfChancesLeft = 2;
        HagglingInitiated?.Invoke();
        _basePrice = _npcBehaviour.GetItemToBuy().FinalPrice;
        _currentPrice = _npcBehaviour.GetItemToBuy().FinalPrice;
    }

    public void TryToSell(){
        int markupPoints = (int)Mathf.Floor(((float)_currentPrice / _basePrice * 100 - 100) / 10);

        if (markupPoints < 0 || markupPoints <= _npcBehaviour.GetTolerance()){
            
            SellItem();
        }
        else{
            int successPoints = 10 + markupPoints - _npcBehaviour.GetTolerance();
            if (Random.Range(1,21) >= successPoints){
                SellItem();
            }
            else if (_nrOfChancesLeft>0)
            {
                _nrOfChancesLeft--;
            }
            else{
                DenySell();
            }
        }
        
    }

    private void SellItem(){
        MoneyManager.MoneyManagerInstance.PutMoney(_currentPrice);
        ItemSold?.Invoke();
        _npcBehaviour.GetDisplaySlotController().RemoveItem();
        EndHaggling();
    }

    private void DenySell(){
        ItemDenied?.Invoke();
        EndHaggling();
    }

    private void EndHaggling(){
        _npcBehaviour.GoToExit();
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

    public int getCurrentPrice()
    {
        return _currentPrice;
    }
    public ItemData getItem()
    {
        return _npcBehaviour.GetItemToBuy();
    }
    public void setCurrentPrice(int price)
    {
        _currentPrice = price;
        PriceChanged?.Invoke();
    }
}
