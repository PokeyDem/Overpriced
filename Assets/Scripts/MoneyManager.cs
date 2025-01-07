using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager MoneyManagerInstance;
    
    [SerializeField]private int _money;

    public UnityEvent changeEvent;
    
    public void Awake() {
        if (MoneyManagerInstance == null) {
            MoneyManagerInstance = this;
            DontDestroyOnLoad(this);
                
        }
    }

    public int GetCurrentMoney() {
        return _money;
    }

    public void PutMoney(int amount) {
        _money += amount;
        changeEvent.Invoke();
    }
    
    public void ReduceMoney(int amount) {
        if (_money >= amount) {
            _money -= amount;
            changeEvent.Invoke();
        }
    } 

    public int GetAmountOfMoney(int amount) {
        if (_money >= amount) {
            ReduceMoney(amount);
            return amount;
        }

        throw new Exception("Not Enough Money");
    }
}
