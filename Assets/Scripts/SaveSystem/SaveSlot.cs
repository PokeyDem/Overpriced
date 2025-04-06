using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour{
    private TextMeshProUGUI _textField;
    private Image _outline;
    private int _id;

    private void Awake(){
        _textField = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _outline = transform.Find("Outline").GetComponentInChildren<Image>();
        
    }

    public void SetId(int id){
        _id = id;
    }

    public int GetId(){
        return _id;
    }

    public void SetSaveInfo(){
        _textField.text = $"Save {_id}\n" 
                          + $"Day: {DayManager.DayManagerInstance.GetDayData().DayCount}\n"
                          + $"Money: {MoneyManager.MoneyManagerInstance.GetMoneyData().Money}\n"
                          + $"Save time: {DateTime.Now:HH:mm}\n"
                          + $"Save date: {DateTime.Now:dd-MM-yyyy}";
    }

    public bool IsEmpty(){
        return _textField.text.Length == 0;
    }

    public void EnableOutline(){
        _outline.gameObject.SetActive(true);
    }

    public void DisableOutline(){
        _outline.gameObject.SetActive(false);
    }
}
