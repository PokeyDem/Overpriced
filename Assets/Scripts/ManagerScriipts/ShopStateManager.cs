using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopStateManager : MonoBehaviour
{
    public static ShopStateManager ShopStateManagerInstance;
    private ShopState _currentShopState=ShopState.Close;
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
        shopStateChange.Invoke(_currentShopState.ToString());
    }

    public void OpenShop() {
        _currentShopState = ShopState.Open;
        shopStateChange.Invoke(_currentShopState.ToString());
        shopWosOpen.Invoke();
    }
    
    public void CloseShop() {
        _currentShopState = ShopState.Close;
        shopStateChange.Invoke(_currentShopState.ToString());
        shopWosClose.Invoke();
    }
}
