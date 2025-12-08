using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.DocumentationSortingAttribute;

public class ExperienceManager : MonoBehaviour {
    public static ExperienceManager ExperienceManagerInstance;
    [SerializeField]private int maxLevel=5;
    [SerializeField] private int _level=1;
    private float _currentExp=0;
    private int _nextLvlExp;
    public UnityEvent<String> levelUpEvent;
    public UnityEvent<float> currentExpEvent;
    
    public void Awake() {
        if (ExperienceManagerInstance == null) {
            ExperienceManagerInstance = this;
            DontDestroyOnLoad(this);
            _nextLvlExp = (int)((1.0 / 4) * (_level + 300 * Math.Pow(2, ((double)_level / 7))));
        }
    }

    public void Start() {
        levelUpEvent.Invoke(_level.ToString());
        currentExpEvent?.Invoke(_currentExp / _nextLvlExp);
    }

    public ExperienceData GetExperienceData(){
        return new ExperienceData(_level, _currentExp);
    }

    public void LoadExperienceData(ExperienceData data){
        _level = data.Level;
        _currentExp = data.CurrentExp;
        currentExpEvent.Invoke(_currentExp);
        levelUpEvent.Invoke(_level.ToString());
    }

    //public void GiveExp(float exp) {
    //    if (_level == maxLevel) {
    //        return;
    //    }
    //    if (_currentExp+exp > _nextLvlExp) {
    //        _level++;
    //        _nextLvlExp += (int)((1.0/4)*(_level+300*Math.Pow(2,((double)_level/7))));
    //        levelUpEvent.Invoke(_level.ToString());
    //        GiveExp((_currentExp + exp) - _nextLvlExp);
    //    }else {
    //        _currentExp += exp;
    //        currentExpEvent.Invoke(_currentExp/_nextLvlExp);
    //    }
        
    //}
    public void GiveExp2(float amount)
    {
        if (_level == maxLevel)
        {
            return;
        }
        _currentExp += amount;

        while (_currentExp >= _nextLvlExp)
        {
            LevelUp();
        }
        currentExpEvent.Invoke(_currentExp / _nextLvlExp);
    }

    private void LevelUp()
    {
        _currentExp -= _nextLvlExp;
        _level++;
        _nextLvlExp = (int)((1.0 / 4) * (_level + 300 * Math.Pow(2, ((double)_level / 7))));
        levelUpEvent.Invoke(_level.ToString());
    }

    public double GetLevel() {
        return _level;
    }
    public float GetCurrentExp() { return _currentExp; }
    public int GetNextLvlExp() { return _nextLvlExp; }

    public void ResetLevel()
    {
        _level = 1;
        _currentExp = 0;
        levelUpEvent.Invoke(_level.ToString());
    }

}
