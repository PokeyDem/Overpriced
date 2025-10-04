using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : SingletonDontDestroyOnLoad<MoneyManager> {
    
    [SerializeField]private int money;

    public UnityEvent<string> changeEvent;
    public UnityEvent<int> onEarnedEvent;
    public UnityEvent<int> onSpendEvent;

    public MoneyData GetMoneyData(){
        return new MoneyData(money);
    }

    public void LoadMoneyData(MoneyData moneyData){
        money = moneyData.Money;
        changeEvent.Invoke(money.ToString());
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
        onEarnedEvent.Invoke(amount);
    }
    
    public void ReduceMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            changeEvent.Invoke(money.ToString());
            onSpendEvent.Invoke(amount);
        }
    }

    public void SetMoney(int amount){
        money = amount;
        changeEvent.Invoke(money.ToString());
    }

    public int GetAmountOfMoney(int amount) {
        if (money >= amount) {
            ReduceMoney(amount);
            return amount;
        }

        throw new Exception("Not Enough Money");
    }
}
