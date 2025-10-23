using DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class HagglingManager : MonoBehaviour, IInteractable, INPCDataUser, IDependencyProvider
{
    [SerializeField] private bool _isPlayerReady;
    [SerializeField] private bool _isNpcReady;
    //private NPCBehaviorTree _npcBehaviour;
    private int _currentPrice;
    private int _basePrice;
    [SerializeField] private int _nrOfAttemptsLeft=3;
    [SerializeField] private bool _hagglingInProgress = false;
    [SerializeField] private int _npcOffer = 0;
    [SerializeField] private PlayerControl _playerControl;


    public UnityEvent HagglingInitiated;
    public UnityEvent HagglingEnded;
    public UnityEvent PriceChanged;
    public UnityEvent NrOfAttemptsChanged;
    public UnityEvent UIDisabled;

    public UnityEvent ItemSold;
    public UnityEvent<float> onItemSold;
    public UnityEvent ItemDenied;
    public UnityEvent AttemptsDepleted;
    public UnityEvent<NPCType> onItemSoldNPCType;


    private float _toleranceDecimal;
    private float _randomDeviation;
    private float _maxThreshold;
    private float _maxAcceptableMarkup;


    private IHasDisplayTarget _displayTargetActor;
    private IMoodController _moodController;
    private IHaggler _haggler;
    private NPCType _npcType;

    private void OnEnable()
    {
        NpcManager.OnNpcReadyToHaggle += SetNpc;
    }

    private void OnDisable()
    {
        NpcManager.OnNpcReadyToHaggle -= SetNpc;
    }



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

    public void SetNpc(IHaggler haggler,IHasDisplayTarget displayTargetActor, float toleranceDecimal, NPCType npcType, IMoodController moodController){
        _haggler = haggler;
        _displayTargetActor = displayTargetActor;
        _moodController = moodController;
        _toleranceDecimal=toleranceDecimal;
        _npcType = npcType;
        SetNpcReadiness(true);
    }
    public void PriceChange()
    {
        PriceChanged?.Invoke();
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
        //if(_npcBehaviour==null || _displayTargetActor.DisplayTarget.GetItem()==null)
        //{
        //    _haggler.IsHaggling=false;
        //    return;
        //}
        _hagglingInProgress = true;
        _nrOfAttemptsLeft = 3;
        _basePrice = _displayTargetActor.DisplayTarget.Info.ItemData.FinalPrice;
        _currentPrice = _basePrice;
        _npcOffer = _basePrice;

        _randomDeviation = Random.Range(350, 480);
        _randomDeviation = _randomDeviation / 100;
        _maxThreshold = (int)(_basePrice + _basePrice * _toleranceDecimal * _randomDeviation);
        float random = Random.value;
        //float bias = random * random;
        _maxAcceptableMarkup = Mathf.Lerp(_toleranceDecimal, _toleranceDecimal * 2.5f, random);
        _playerControl.enabled = false;
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
            Debug.Log("chance: "+ _maxAcceptableMarkup + ", increase: "+percentageIncreaseDecimal);
            if (percentageIncreaseDecimal <= _maxAcceptableMarkup)
            {
                SellItem();
            }
            else if (_nrOfAttemptsLeft > 1 && _currentPrice<= _maxThreshold)
            {
                _nrOfAttemptsLeft--;
                _npcOffer = (int)(_basePrice + Mathf.Round(_basePrice* _maxAcceptableMarkup/6 * (3-_nrOfAttemptsLeft)));
                NrOfAttemptsChanged?.Invoke();
            }
            else{
                _nrOfAttemptsLeft--;
                _npcOffer = (int)(_basePrice + Mathf.Round(_basePrice * _maxAcceptableMarkup / 6 * (3 - _nrOfAttemptsLeft)));
                _nrOfAttemptsLeft = 0;
                _currentPrice =_npcOffer;
                PriceChanged?.Invoke();
                NrOfAttemptsChanged?.Invoke();
                AttemptsDepleted?.Invoke();
            }
        }
        
    }

    public void SellItem(){
        MoneyManager.Instance.PutMoney(_currentPrice);
        ItemSold?.Invoke();
        onItemSold?.Invoke(_currentPrice*_toleranceDecimal);
        onItemSoldNPCType?.Invoke(_npcType);
        _displayTargetActor.DisplayTarget.Editor.RemoveItem();
        _moodController.InvokeMoodChange(MoodType.Happy);
        StartCoroutine(EndHaggling());
    }

    public void DenySell(){
        _nrOfAttemptsLeft = 0;
        ItemDenied?.Invoke();
        _moodController.InvokeMoodChange(MoodType.Angry);
        StartCoroutine(EndHaggling());
    }

    private IEnumerator EndHaggling(){
        UIDisabled?.Invoke();
        yield return new WaitForSeconds(1);
        _haggler.IsHaggling = false;
        _displayTargetActor.DisplayTarget.Flags.IsChosen = false;
        _displayTargetActor.DisplayTarget.Flags.IsOccupied = false;
        HagglingEnded?.Invoke();
        _hagglingInProgress = false;
        _playerControl.enabled = true;
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
        return _displayTargetActor.DisplayTarget.Info.ItemData;
    }
    public void SetCurrentPrice(int price)
    {
        _currentPrice = price;
    }
    //public NPCBehaviorTree GetNpc()
    //{
    //    return _npcBehaviour;
    //}
    public int GetNrOfAttemptsLeft()
    {
        return _nrOfAttemptsLeft;
    }
    public int GetNpcOffer()
    {
        return _npcOffer;
    }
    public float GetMaxAcceptableMarkup()
    {
        return _maxAcceptableMarkup;
    }


}
