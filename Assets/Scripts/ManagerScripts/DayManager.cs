using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : SingletonDontDestroyOnLoad<DayManager>{
        
    [SerializeField] private int dayCount=1;
    [SerializeField] private PartOfDay currentPartOfDay=PartOfDay.Morning;
    public UnityEvent<string> partOfDayChange;
    public UnityEvent<int> dayChange;
        
    public enum PartOfDay {
        Morning=0, Noon=1, Evening=2
    }

    public void Start() {
        dayChange?.Invoke(dayCount);
        partOfDayChange?.Invoke(currentPartOfDay.ToString());
    }

    private void Update(){ //TODO for development purposes
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            NextPartOfTheDay();
    }

    public DayData GetDayData(){
        return new DayData(dayCount, currentPartOfDay);
    }

    public int GetDay(){
        return dayCount;
    }

    public PartOfDay GetPartOfDay(){
        return currentPartOfDay;
    }

    public void LoadDayData(DayData dayData){
        dayCount = dayData.DayCount;
        currentPartOfDay = dayData.DayPart;
        dayChange.Invoke(dayCount);
        partOfDayChange.Invoke(currentPartOfDay.ToString());
    }
    
    public void NextPartOfTheDay() {
        // if (currentPartOfDay != PartOfDay.Noon) {
        //     currentPartOfDay++;
        // }else {
        //     currentPartOfDay = PartOfDay.Morning;
        //     dayCount++;
        //     dayChange.Invoke(dayCount);
        // }

        if (currentPartOfDay != PartOfDay.Evening)
            currentPartOfDay++;
        else if (currentPartOfDay == PartOfDay.Evening){
            currentPartOfDay = PartOfDay.Morning;
            dayCount++;
            dayChange.Invoke(dayCount);
        }
        partOfDayChange.Invoke(currentPartOfDay.ToString());
        LightingManager.Instance.SetLighting(currentPartOfDay);
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
