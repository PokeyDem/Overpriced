using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager MoneyManagerInstance;
    
    [SerializeField]private int money;

    public UnityEvent<string> changeEvent;
    
    public void Awake() {
        if (MoneyManagerInstance == null) {
            MoneyManagerInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void Start() {
        changeEvent.Invoke(money.ToString());
    }

    public int GetCurrentMoney() {
        return money;
    }

    public void PutMoney(int amount) {
        money += amount;
        changeEvent.Invoke(money.ToString());
    }
    
    public void ReduceMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            changeEvent.Invoke(money.ToString());
        }
    } 

    public int GetAmountOfMoney(int amount) {
        if (money >= amount) {
            ReduceMoney(amount);
            return amount;
        }

        throw new Exception("Not Enough Money");
    }
}
