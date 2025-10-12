using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisplaySlotController : MonoBehaviour{
    [SerializeField] private GameObject _marker;
    [SerializeField] private GameObject priseUI;
    [SerializeField] private bool isBought;
    [SerializeField] private int prize;
    public bool IsBought => isBought;
    private int _displayTypeID;
    private float _rotationSpeed = 45f;
    private GameObject _itemPrefab;
    private ItemData _item;
    public bool isOccupied=false;//by npc
    public bool isChosen=false;
    private Vector3 _position;
    public UnityEvent<String> onTextUpdate;


    private void Update(){
        if (_itemPrefab)
            _itemPrefab.transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")) {
            if (isBought) {
                _marker.SetActive(true);
            }
            else {
                priseUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            if (isBought) {
                _marker.SetActive(false);
            }
            else {
                priseUI.SetActive(false);
            }
        }
    }

    private void Awake(){
        _marker.SetActive(false);
        priseUI.SetActive(false);
        onTextUpdate.Invoke(prize.ToString());
    }

    public void EnableMarker(){
        if (isBought) {
            _marker.SetActive(true);
        }
        else {
            priseUI.SetActive(true);
        }
    }

    public void DisableMarker(){
        _marker.SetActive(false);
        priseUI.SetActive(false);
    }

    public void PlaceItem(ItemData item){
        if (!isBought) {
            return;
        }

        if (_itemPrefab != null){
            Destroy(_itemPrefab);
        }
        _itemPrefab = Instantiate(item.Prefab, _marker.transform.position, Quaternion.identity);
        _item = item;
        isChosen = false;
        isOccupied = false;
        DisplaysWithItemsListHandler.Instance.AddDisplaySlotWithItem(this);
    }

    public void RemoveItem(){
        Destroy(_itemPrefab);
        _itemPrefab = null;
        _item = null;
        DisplaysWithItemsListHandler.Instance.RemoveDisplaySlotWithItem(this);
    }

    public int GetItemId(){
        if (_item==null) {
            return -1;
        }
        return _item.ID;
    }
    public ItemData GetItem()
    {
        return _item;
    }

    public void SetDisplayTypeId(int id){
        _displayTypeID = id;
    }

    public int GetDisplayTypeId(){
        return _displayTypeID;
    }

    public void SetPosition(Vector3 position){
        _position = position;
    }

    public Vector3 GetPosition(){
        return _position;
    }

    public void TryToBuy() {
        MoneyManager moneyManager = MoneyManager.Instance;
        if (moneyManager.GetCurrentMoney() >= prize) {
            moneyManager.ReduceMoney(prize);
            isBought = true;
        }
    }
}
