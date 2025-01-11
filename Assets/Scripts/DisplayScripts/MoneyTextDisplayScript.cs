using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTextDisplayScript : UpdatableDisplay {

    public void Start() {
        textField.text = MoneyManager.MoneyManagerInstance.GetCurrentMoney().ToString();
    }

    public override void UpdateText() {
        textField.text = MoneyManager.MoneyManagerInstance.GetCurrentMoney().ToString();
    }
}
