using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySlotController : MonoBehaviour{
    [SerializeField] private GameObject _marker;
    private int _displayTypeID;
    private float _rotationSpeed = 45f;
    private GameObject _itemPrefab;
    private int _itemId = -1;
    [SerializeField] private ItemData _item;
    public bool isOccupied=false;//by npc
    public bool isChosen=false;
    private Vector3 _position;


    private void Update(){
        if (_itemPrefab)
            _itemPrefab.transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
            _marker.SetActive(true);
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            _marker.SetActive(false);
        }
    }

    private void Awake(){
        _marker.SetActive(false);
    }

    public void EnableMarker(){
        _marker.SetActive(true);
    }

    public void DisableMarker(){
        _marker.SetActive(false);
    }

    public void PlaceItem(ItemData item){
        if (_itemPrefab != null && _itemId == item.ID){
            Destroy(_itemPrefab);
        }
        _itemPrefab = Instantiate(item.Prefab, _marker.transform.position, Quaternion.identity);
        _itemId = item.ID;
        _item = item;
        isChosen = false;
        isOccupied = false;
        DisplaysWithItemsListHandler.Instance.AddDisplaySlotWithItem(this);
    }

    public void RemoveItem(){
        Destroy(_itemPrefab);
        _itemPrefab = null;
        _item = null;
        _itemId = -1;
        DisplaysWithItemsListHandler.Instance.RemoveDisplaySlotWithItem(this);
    }

    public int GetItemId(){
        return _itemId;
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
}
