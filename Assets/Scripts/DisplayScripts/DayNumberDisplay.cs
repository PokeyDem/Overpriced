using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNumberDisplay : UpdatableDisplay {
    
    
    // Start is called before the first frame update
    void Start() {
        textField.text = DayManager.DayManagerInstance.GetDay().ToString();
    }

    public override void UpdateText() {
        textField.text = DayManager.DayManagerInstance.GetDay().ToString();
    }
}
