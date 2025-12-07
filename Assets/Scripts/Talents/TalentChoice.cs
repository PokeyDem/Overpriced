using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TalentChoice : MonoBehaviour
{
    [SerializeField] private Button buttonA;
    [SerializeField] private Talent talentA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Talent talentB;
    [SerializeField] private TalentChoice predecessor;
    public UnityEvent OnUnlock;
    public bool IsUnlock  { get; private set; } = false;
    public bool CanBeUnlock { get; private set; }  = true;

    private void Awake() {
        //icon.GetComponent<SpriteRenderer>().sprite = talentA.icon;
        buttonA.onClick.AddListener(UnlockA);
        buttonB.onClick.AddListener(UnlockB);
        if (predecessor!=null && !predecessor.IsUnlock) {
            buttonA.interactable = false;
            buttonB.interactable = false;
            CanBeUnlock = false;
        }
    }

    public void UnlockA() {
        Unlock(talentA);
    }

    public void UnlockB()
    {
        Unlock(talentB);
    }

    private void Unlock(Talent talent) {
        if (CanBeUnlock && TalentPointsManager.Instance.HasSkillPoints){
            TalentPointsManager.Instance.UseSkillPoint();
            CanBeUnlock = false;
            IsUnlock =  true;
            talent.OnUnlock?.Invoke();
            buttonA.interactable = false;
            buttonB.interactable = false;
        }
    }

    private void Activate() {
        buttonA.interactable = true;
        buttonB.interactable = true;
        CanBeUnlock = true;
    }

}
