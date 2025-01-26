using System.Collections;
using TMPro;
using UnityEngine;


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
        _basePrice = _npcBehaviour.GetItemToBuy().Price;
        _itemDescField.text = new string("Item name:\n" + _npcBehaviour.GetItemToBuy().Name + "\nDescription:\n" + _npcBehaviour.GetItemToBuy().Description + "\nBase price:\n" + _basePrice);
        _currentPrice = _basePrice;
        _counterField.text = _basePrice.ToString();
        _resulField.text = "";
    }

    public void TryToSell(){
        int markupPoints = (_currentPrice - _basePrice) / 10;
        

        if (markupPoints < 0 || markupPoints <= _npcBehaviour.GetTolerance()){
            Debug.Log("Sold on stage 1 | MarkupPoints: " + markupPoints);
            SellItem();
        }
        else{
            int successPoints = 10 + markupPoints - _npcBehaviour.GetTolerance();
            int random = Random.Range(1, 20); //TODO for debug purposes move to if later
            if (random >= successPoints){
                Debug.Log("Sold on stage 2 | successPoints: " + successPoints + " | Random: " + random);
                SellItem();
            }
            else{
                Debug.Log("Sell failed | successPoints: " + successPoints + " | Random: " + random);
                DenySell();
            }
        }
        
        EndHaggling();
        
    }

    private void SellItem(){
        MoneyManager.MoneyManagerInstance.PutMoney(_currentPrice);
        _resulField.color = Color.green;
        _resulField.text = "Sold";
        _npcBehaviour.GetDisplay().GetComponentInChildren<DisplaySlotController>().RemoveItem();
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
