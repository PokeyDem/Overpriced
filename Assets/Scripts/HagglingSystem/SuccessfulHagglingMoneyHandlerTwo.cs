using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuccessfulHagglingMoneyHandlerTwo : MonoBehaviour
{
    [SerializeField] private HagglingManager _manager;
    [SerializeField] private GameObject _moneyGainedContainer;
    [SerializeField] private GameObject _moneyGainedNumberObject;

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
        GameObject textGO = Instantiate(_moneyGainedNumberObject);
        textGO.transform.SetParent(_moneyGainedContainer.transform, false);
        TextMeshProUGUI text = textGO.GetComponent<TextMeshProUGUI>();
        text.text = $"+{amount}";
        StartCoroutine(MoneyGainedCoroutine(text));
    }

    public IEnumerator MoneyGainedCoroutine(TextMeshProUGUI text)
    {
        text.alpha = 1f;
        while (text.alpha>0)
        {
            text.alpha -= 0.02f;
            if (text.alpha < 0) text.alpha = 0;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(text.gameObject);
    }

}
