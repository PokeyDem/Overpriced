using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour {
    public static DayManager DayManagerInstance;
        
    [SerializeField] private int dayCount=1;
    [SerializeField] private PartOfDay currentPartOfDay=PartOfDay.Morning;
    public UnityEvent<string> partOfDayChange;
    public UnityEvent<int> dayChange;
        
    public enum PartOfDay {
        Morning=0, Noon=1, Evening=2, Dusk=3
    }
    
    // Start is called before the first frame update
    public void Awake() {
        if (DayManagerInstance == null) {
            DayManagerInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void Start() {
        dayChange.Invoke(dayCount);
        partOfDayChange.Invoke(currentPartOfDay.ToString());
    }

    public int GetDay() {
        return dayCount;
    }

    public PartOfDay GetPartOfDay() {
        return currentPartOfDay;
    }
    
    public void NextPartOfTheDay() {
        if (currentPartOfDay != PartOfDay.Dusk) {
            currentPartOfDay++;
        }else {
            currentPartOfDay = PartOfDay.Morning;
            dayCount++;
            dayChange.Invoke(dayCount);
        }
        partOfDayChange.Invoke(currentPartOfDay.ToString());
    }

    public void SkipPartsOfTheDay(int amount) {
        if (amount == 1) {
            NextPartOfTheDay();
        }else if (amount > 1) {
            while (amount > 0) {
                NextPartOfTheDay();
                amount--;
            }
        }
    }

    public void SkipDay() {
        dayCount++;
        currentPartOfDay = PartOfDay.Morning;
        dayChange.Invoke(dayCount);
    } 

}
