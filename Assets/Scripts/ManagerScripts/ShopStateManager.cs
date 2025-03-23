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

    public enum ShopState {
        Open, Close
    }
    
    public void Awake() {
        if (ShopStateManagerInstance == null) {
            ShopStateManagerInstance = this;
            DontDestroyOnLoad(this);
        }
    }
    
    void Start() {
        _currentShopState = ShopState.Close;
        shopStateChange.Invoke(_currentShopState.ToString());
    }
    
    private float _openTimeCounter;
    
    public void OpenShop() {
        _currentShopState = ShopState.Open;
        shopStateChange.Invoke(_currentShopState.ToString());
        shopWosOpen.Invoke();
        _openTimeCounter = openShopTimeSeconds;
    }

    public bool ShopIsClose() {
        return _currentShopState == ShopState.Close;
    }
    
    public void CloseShop() {
        _currentShopState = ShopState.Close;
        shopStateChange.Invoke(_currentShopState.ToString());
        shopWosClose.Invoke();
    }

    private bool _timeStop=false;
    
    private void Update() {
        if (_timeStop) {
            return;
        }
        if (_openTimeCounter>0) {
            _openTimeCounter -= Time.deltaTime;
        }else if(_currentShopState==ShopState.Open) {
            CloseShop();
        }
    }

    public void StopTime() {
        _timeStop = true;
    }
    
    public void ResumeTime() {
        _timeStop = false;
    }
}
