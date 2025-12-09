using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TalentPointsManager : SingletonDontDestroyOnLoad<TalentPointsManager>
{
    
    [SerializeField] private int skillPoints;
    [SerializeField] private UnityEvent<string> OnSkillPointsChanged;
    public bool HasSkillPoints => skillPoints > 0;

    private void Awake() {
        base.Awake();
        OnSkillPointsChanged?.Invoke(skillPoints.ToString());
    }

    public void AddSkillPoint() {
        skillPoints++;
        OnSkillPointsChanged?.Invoke(skillPoints.ToString());
    }

    public void UseSkillPoint() {
        skillPoints--;
        OnSkillPointsChanged?.Invoke(skillPoints.ToString());
    }
}
