using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour{
    private TextMeshProUGUI _textField;
    private Image _outline;
    private int _id;

    private void Awake(){
       InitializeButton();
    }

    public void InitializeButton(){
        _textField = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _outline = transform.Find("Outline").GetComponentInChildren<Image>();
    }

    public void SetId(int id){
        _id = id;
    }

    public int GetId(){
        return _id;
    }

    public void SetSaveInfo(){ //TODO refactor this later
        Debug.Log(_id);
        if (_id == 6)
            _textField.text = "QuickSave\n"
                              + $"Day: {DayManager.Instance.GetDayData().DayCount}\n"
                              + $"Money: {MoneyManager.Instance.GetMoneyData().Money}\n";
        else if (_id == 7)
            _textField.text = "AutoSave\n"
                              + $"Day: {DayManager.Instance.GetDayData().DayCount}\n"
                              + $"Money: {MoneyManager.Instance.GetMoneyData().Money}\n";
        else
            _textField.text = $"Save {_id}\n" 
                          + $"Day: {DayManager.Instance.GetDayData().DayCount}\n"
                          + $"Money: {MoneyManager.Instance.GetMoneyData().Money}\n"
                          + $"Save time: {DateTime.Now:HH:mm}\n"
                          + $"Save date: {DateTime.Now:dd-MM-yyyy}";
    }

    public void SetSaveInfo(String info){
        _textField.text = info;
    }

    public String GetSaveInfo(){
        return _textField.text;
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
