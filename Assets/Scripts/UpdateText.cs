using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _text.gameObject.SetActive(false);
    }

    public void UpdateTextMesh(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
    }
    public void SetActive(bool isActive)
    {
        _text.gameObject.SetActive(isActive);
    }

}
