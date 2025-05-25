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
        LinearColor linearColor = new LinearColor();
        linearColor.red = _morningDirectionalColor.r;
        linearColor.green = _morningDirectionalColor.g;
        linearColor.blue = _morningDirectionalColor.b;
        linearColor.intensity = _morningDirectionalIntensity;

        _directionalLight.color = linearColor;
    }

    private void SetNoonLighting(){
        LinearColor linearColor = new LinearColor();
        linearColor.red = _noonDirectionalColor.r;
        linearColor.green = _noonDirectionalColor.g;
        linearColor.blue = _noonDirectionalColor.b;
        linearColor.intensity = _noonDirectionalIntensity;

        _directionalLight.color = linearColor;
    }

    private void SetEveningLighting(){
        LinearColor linearColor = new LinearColor();
        linearColor.red = _eveningDirectionalColor.r;
        linearColor.green = _eveningDirectionalColor.g;
        linearColor.blue = _eveningDirectionalColor.b;
        linearColor.intensity = _eveningDirectionalIntensity;

        _directionalLight.color = linearColor;
    }
}
