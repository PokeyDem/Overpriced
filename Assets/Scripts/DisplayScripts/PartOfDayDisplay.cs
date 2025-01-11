using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfDayDisplay : UpdatableDisplay
{
    // Start is called before the first frame update
    void Start()
    {
        textField.text = DayManager.DayManagerInstance.GetPartOfDay().ToString();
    }

    public override void UpdateText() {
        textField.text = DayManager.DayManagerInstance.GetPartOfDay().ToString();
    }
}
