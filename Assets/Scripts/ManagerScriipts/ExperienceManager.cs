using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExperienceManager : MonoBehaviour {
    public static ExperienceManager ExperienceManagerInstance;
    
    public void Awake() {
        if (ExperienceManagerInstance == null) {
            ExperienceManagerInstance = this;
            DontDestroyOnLoad(this);
            _nextLvlExp = (int)((1.0 / 4) * (_level + 300 * Math.Pow(2, ((double)_level / 7))));
        }
    }

    public void Start() {
        levelUpEvent.Invoke(_level.ToString());
    }

    [SerializeField]private int maxLevel=5;
    private int _level=1;
    private float _currentExp=0;
    private int _nextLvlExp;
    public UnityEvent<String> levelUpEvent;
    public UnityEvent<float> currentExpEvent;

    public void GiveExp(float exp) {
        if (_level == maxLevel) {
            return;
        }
        if (_currentExp+exp > _nextLvlExp) {
            _level++;
            _nextLvlExp += (int)((1.0/4)*(_level+300*Math.Pow(2,((double)_level/7))));
            levelUpEvent.Invoke(_level.ToString());
            GiveExp((_currentExp + exp) - _nextLvlExp);
        }else {
            _currentExp += exp;
            currentExpEvent.Invoke(_currentExp/_nextLvlExp);
        }
        
    }

    public double GetLevel() {
        return _level;
    }
}
