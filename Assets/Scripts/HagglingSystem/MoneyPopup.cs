using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPopup : MonoBehaviour
{
    [SerializeField] private GameObject _moneyGainedContainer;
    [SerializeField] private GameObject _moneyGainedNumberObject;

    public void SpawnMoneyPopup(int amount)
    {
        GameObject textGO = Instantiate(_moneyGainedNumberObject);
        textGO.transform.SetParent(_moneyGainedContainer.transform, false);
        TextMeshProUGUI text = textGO.GetComponent<TextMeshProUGUI>();
        if(amount<0)
        {
            text.text = $"{amount}";
        } else
        {
            text.text = $"+{amount}";
        }
        StartCoroutine(SpawnMoneyPopup(text));
    }

    private IEnumerator SpawnMoneyPopup(TextMeshProUGUI text)
    {
        text.alpha = 1f;
        while (text.alpha>0)
        {
            yield return new WaitForSeconds(0.05f);
            text.alpha -= 0.02f;
            if (text.alpha < 0) text.alpha = 0;
        }
        Destroy(text.gameObject);
    }

}
