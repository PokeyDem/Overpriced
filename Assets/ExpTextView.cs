using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _expText;
    public void UpdateExpText(int current, int max)
    {
        _expText.text = current + "/" + max+ " XP";
    }
}
