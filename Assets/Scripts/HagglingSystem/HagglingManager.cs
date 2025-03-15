using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class HagglingManager : MonoBehaviour{
    private bool _isPlayerReady;
    private bool _isNpcReady;
    private NpcBehaviour _npcBehaviour;
    [SerializeField] private Canvas _hagglingUI;
    [SerializeField] TextMeshProUGUI _counterField;
    [SerializeField] TextMeshProUGUI _itemDescField;
    [SerializeField] TextMeshProUGUI _resulField;
    private int _currentPrice;
    private int _basePrice;

    private void Awake(){
        _hagglingUI.gameObject.SetActive(false);
    }

    public void SetPlayerReadiness(bool isPlayerReady){
        _isPlayerReady = isPlayerReady;
        CheckReadiness();
    }

    public void SetNpcReadiness(bool isNpcReady){
        _isNpcReady = isNpcReady;
        CheckReadiness();
    }

    private void CheckReadiness(){
        if (_isPlayerReady && _isNpcReady){
            _hagglingUI.gameObject.SetActive(true);
            StartHaggling();
        }
    }

    public void SetNpc(NpcBehaviour npcBehaviour){
        _npcBehaviour = npcBehaviour;
    }

    public void IncreaseCounter(){
        if (_currentPrice < _basePrice * 2){
            _currentPrice += 10;
            _counterField.text = _currentPrice.ToString();
        }
    }

    public void DecreaseCounter(){
        if (_currentPrice > 10){
            _currentPrice -= 10;
            _counterField.text = _currentPrice.ToString();
        }
    }

    public void StartHaggling(){
        _basePrice = _npcBehaviour.GetItemToBuy().FinalPrice;
        _itemDescField.text = new string("Item name:\n" + _npcBehaviour.GetItemToBuy().Name + "\nDescription:\n" + _npcBehaviour.GetItemToBuy().Description + "\nBase price:\n" + _basePrice);
        _currentPrice = _basePrice;
        _counterField.text = _basePrice.ToString();
        _resulField.text = "";
    }

    public void TryToSell(){
        _currentPrice = Int32.Parse(_counterField.text);
        Debug.Log(_currentPrice);
        Debug.Log(_basePrice);
        int markupPoints = (int)Mathf.Floor(((float)_currentPrice / _basePrice * 100 - 100) / 10);

        if (markupPoints < 0 || markupPoints <= _npcBehaviour.GetTolerance()){
            
            SellItem();
        }
        else{
            int successPoints = 10 + markupPoints - _npcBehaviour.GetTolerance();
            if (Random.Range(1,21) >= successPoints){
                SellItem();
            }
            else{
                DenySell();
            }
        }
        
        EndHaggling();
        
    }

    private void SellItem(){
        MoneyManager.MoneyManagerInstance.PutMoney(_currentPrice);
        _resulField.color = Color.green;
        _resulField.text = "Sold";
        _npcBehaviour.GetDisplayItemSlot().GetComponentInChildren<DisplaySlotController>().RemoveItem();
    }

    private void DenySell(){
        _resulField.color = Color.red;
        _resulField.text = "Failed";
    }

    private void EndHaggling(){
        _npcBehaviour.GoToExit();
        StartCoroutine(DisableUiDelay());
    }
    
    private IEnumerator DisableUiDelay(){
        yield return new WaitForSeconds(1);
        _hagglingUI.gameObject.SetActive(false);
    }
}
