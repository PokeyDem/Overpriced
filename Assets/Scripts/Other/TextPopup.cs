using ManagerScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField] GameObject _textPrefab;
    [SerializeField] GameObject _textPopupContainer;
    public void SpawnPopup(string text)
    {
        GameObject textGO = Instantiate(_textPrefab);
        TextMeshProUGUI tmp=textGO.GetComponentInChildren<TextMeshProUGUI>();
        textGO.transform.SetParent(_textPopupContainer.transform, false);
        tmp.text = text;
        CanvasGroup group= textGO.GetComponent<CanvasGroup>();
        StartCoroutine(SpawnPopup(group));
    }

    private IEnumerator SpawnPopup(CanvasGroup group) 
    {

        group.alpha = 1f;
        while (group.alpha > 0)
        {
            yield return new WaitForSeconds(0.05f);
            group.alpha -= 0.02f;
            if (group.alpha < 0) group.alpha = 0;
        }
        Destroy(group.transform.gameObject);
    }
}
