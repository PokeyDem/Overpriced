using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager MoneyManagerInstance;
    
    private int _money;
    
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
    }

    public int GetAmountOfMoney(int amount) {
        if (_money >= amount) {
            _money -= amount;
            return amount;
        }

        throw new Exception("Not Enough Money");
    }
}
