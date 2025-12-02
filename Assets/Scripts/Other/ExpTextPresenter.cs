using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpTextPresenter : MonoBehaviour
{
    [SerializeField] private ExpTextView _view;
    [SerializeField] private ExperienceManager _model;

    private void Awake()
    {
        _model.currentExpEvent.AddListener(OnExpChanged);
    }
    public void OnExpChanged(float whatever)
    {
        _view.UpdateExpText((int)_model.GetCurrentExp(), _model.GetNextLvlExp());
    }
}
