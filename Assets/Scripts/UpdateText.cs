using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void UpdateImage(Sprite emote)
    {
       _image.sprite = emote;
    }
    public void UpdateImageAlpha(float value)
    {
        Color c = _image.color;
        c.a = value;
        _image.color = c;
    }

}
