using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject _interactUICanvas;
    [SerializeField] private TextMeshProUGUI _interactUIText;
    public void UpdateText(string text)
    {
        _interactUIText.text = text;
    }
    public void SetActive(bool isActive)
    {
        if(_interactUICanvas != null)
        {
            _interactUICanvas.SetActive(isActive);
        }
    }
}

