using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuccessfulHagglingMoneyHandler : MonoBehaviour
{
    [SerializeField] private HagglingManager _manager;
    [SerializeField] TextMeshProUGUI _moneyGainedNumber;
    private int _targetNumber;
    private int _step;
    private int _current;

    private void OnEnable()
    {
        _manager.ItemSold.AddListener(MoneyGainedCoroutineHandler);
    }
    private void OnDisable()
    {
        _manager.ItemSold.RemoveListener(MoneyGainedCoroutineHandler);
    }
    public void MoneyGainedCoroutineHandler()
    {
        int amount = _manager.GetCurrentPrice();
        _targetNumber += amount;
        if (_targetNumber == amount)
        {
            StartCoroutine(MoneyGainedCoroutine());
        }
    }
    public IEnumerator MoneyGainedCoroutine()
    {
        _moneyGainedNumber.gameObject.SetActive(true);
        while (_current < _targetNumber)
        {
            _step = (_targetNumber - _current) / 20;
            if (_step == 0)
            {
                _step=1;
            }
            _current += _step;
            if (_current > _targetNumber) _current = _targetNumber;
            _moneyGainedNumber.text = $"+{_current}";
            yield return new WaitForSeconds(0.05f);
        }
        MoneyManager.Instance.PutMoney(_current);
        _targetNumber = 0;
        _current = 0;
        _moneyGainedNumber.gameObject.SetActive(false);
    }
}
