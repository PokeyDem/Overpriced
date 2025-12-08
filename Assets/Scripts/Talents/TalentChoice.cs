using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TalentChoice : MonoBehaviour
{
    [SerializeField] private Button buttonA;
    [SerializeField] private Image imageA;
    [SerializeField] private TextMeshProUGUI  textA;
    [SerializeField] private Talent talentA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Image imageB;
    [SerializeField] private TextMeshProUGUI  textB;
    [SerializeField] private Talent talentB;
    [SerializeField] private TalentChoice predecessor;
    public UnityEvent OnActivate;
    public bool IsUnlock  { get; private set; } = false;
    public bool CanBeUnlock { get; private set; }  = true;

    private void Awake() {
        //icon.GetComponent<SpriteRenderer>().sprite = talentA.icon;
        buttonA.onClick.AddListener(UnlockA);
        imageA.sprite = talentA.icon;
        textA.text = talentA.name;
        buttonB.onClick.AddListener(UnlockB);
        imageB.sprite = talentB.icon;
        textB.text = talentB.name;
        if (predecessor!=null) {
            buttonA.interactable = false;
            buttonB.interactable = false;
            CanBeUnlock = false;
            predecessor.OnActivate.AddListener(Activate);
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
            OnActivate?.Invoke();
            buttonA.interactable = false;
            buttonB.interactable = false;
        }
    }

    public void Activate() {
        buttonA.interactable = true;
        buttonB.interactable = true;
        CanBeUnlock = true;
    }

}
