using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

public class LightingManager : SingletonDontDestroyOnLoad<LightingManager>{
    [SerializeField] private GameObject _directionalLightObject;
    [SerializeField] private Color _morningDirectionalColor;
    [SerializeField] private float _morningDirectionalIntensity;
    
    [SerializeField] private Color _noonDirectionalColor;
    [SerializeField] private float _noonDirectionalIntensity;
    
    [SerializeField] private Color _eveningDirectionalColor;
    [SerializeField] private float _eveningDirectionalIntensity;

    private Light _directionalLight;

    public new void Awake(){
        base.Awake();
        _directionalLight = _directionalLightObject.GetComponent<Light>();
        SetMorningLighting();
    }

    public void SetLighting(DayManager.PartOfDay partOfDay){
        switch (partOfDay){
            case DayManager.PartOfDay.Morning:
                SetMorningLighting();
                break;
            case DayManager.PartOfDay.Noon:
                SetNoonLighting();
                break;
            case DayManager.PartOfDay.Evening:
                SetEveningLighting();
                break;
        }
    }
    
    private void SetMorningLighting(){
       _directionalLight.color = _morningDirectionalColor;
       _directionalLight.intensity = _morningDirectionalIntensity;
    }

    private void SetNoonLighting(){
        _directionalLight.color = _noonDirectionalColor;
        _directionalLight.intensity = _noonDirectionalIntensity;
    }

    private void SetEveningLighting(){
       _directionalLight.color = _eveningDirectionalColor;
       _directionalLight.intensity = _eveningDirectionalIntensity;
    }
}
