using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour{
    [SerializeField] private GameObject _marker;
    private float _rotationSpeed = 45f;
    private GameObject _itemPrefab;
    private int _itemId;
    private bool _isOccupied;

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

    public void PlaceItem(GameObject item, int itemId){
        if (_itemPrefab != null && _itemId == itemId){
            Destroy(_itemPrefab);
        }
        _itemPrefab = Instantiate(item, _marker.transform.position, Quaternion.identity);
        _isOccupied = true;
        _itemId = itemId;
    }

    public void RemoveItem(){
        Destroy(_itemPrefab);
        _itemPrefab = null;
        _isOccupied = false;
        _itemId = -1;
    }
}
