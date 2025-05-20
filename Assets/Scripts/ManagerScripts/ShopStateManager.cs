using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopStateManager : MonoBehaviour
{
    public static ShopStateManager ShopStateManagerInstance;
    private ShopState _currentShopState;
    [SerializeField] private int openShopTimeSeconds;
    public UnityEvent<string> shopStateChange;
    public UnityEvent shopWosOpen;
    public UnityEvent shopWosClose;
    public UnityEvent changePartOfTheDay; //Moved it to the separate event, to be able to implement load method correctly

    public enum ShopState {
        Open, Closed
    }

    public ShopStateData GetShopStateData(){
        return new ShopStateData(_currentShopState, openShopTimeSeconds);
    }

    public void LoadShopState(ShopStateData shopStateData){
        if (_currentShopState != shopStateData.ShopState){
            if (shopStateData.ShopState == ShopState.Closed){
                _currentShopState = ShopState.Closed;
                shopStateChange.Invoke(_currentShopState.ToString());
                shopWosClose.Invoke();
            }
            else
                OpenShop();
        }
    }
    
    public void Awake() {
        if (ShopStateManagerInstance == null) {
            ShopStateManagerInstance = this;
            DontDestroyOnLoad(this);
        }
    }
    
    void Start() {
        _currentShopState = ShopState.Closed;
        shopStateChange.Invoke(_currentShopState.ToString());
    }
    
    private float _openTimeCounter;
    
    public void OpenShop() {
        _currentShopState = ShopState.Open;
        shopStateChange.Invoke(_currentShopState.ToString());
        shopWosOpen.Invoke();
        _openTimeCounter = openShopTimeSeconds;
        SaveUIManager.Instance.DisableMainMenuSaveButtons();
    }

    public bool ShopIsClose() {
        return _currentShopState == ShopState.Closed;
    }
    
    public void CloseShop() {
        _currentShopState = ShopState.Closed;
        shopStateChange.Invoke(_currentShopState.ToString());
        shopWosClose.Invoke();
        changePartOfTheDay.Invoke();
        SaveUIManager.Instance.EnableMainMenuSaveButtons();
    }

    private bool _timeStop=false;
    
    private void Update() {
        //if (_timeStop) {
        //    return;
        //}
        //if (_openTimeCounter>0) {
        //    _openTimeCounter -= Time.deltaTime;
        //}else if(_currentShopState==ShopState.Open) {
        //    CloseShop();
        //}
    }

    public void StopTime() {
        _timeStop = true;
    }
    
    public void ResumeTime() {
        _timeStop = false;
    }
}
