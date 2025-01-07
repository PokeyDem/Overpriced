using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTextDisplayScript : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI TextMoney;

    public void Start() {
        TextMoney.text = MoneyManager.MoneyManagerInstance.GetCurrentMoney().ToString();
    }

    public void setText() {
        TextMoney.text = MoneyManager.MoneyManagerInstance.GetCurrentMoney().ToString();
    }
}
