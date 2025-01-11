using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager MoneyManagerInstance;
    
    [SerializeField]private int money;

    public UnityEvent changeEvent;
    
    public void Awake() {
        if (MoneyManagerInstance == null) {
            MoneyManagerInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public int GetCurrentMoney() {
        return money;
    }

    public void PutMoney(int amount) {
        money += amount;
        changeEvent.Invoke();
    }
    
    public void ReduceMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            changeEvent.Invoke();
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
