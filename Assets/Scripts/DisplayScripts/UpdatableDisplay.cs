using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UpdatableDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textField;

    public abstract void UpdateText();
}
