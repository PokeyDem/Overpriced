using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DynamicDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textField;
    

    public void UpdateText(string text) {
        textField.text = text;
    }
    
    public void UpdateText(int number) {
        textField.text = number.ToString();
    }
}
