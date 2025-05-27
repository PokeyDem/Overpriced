using System;
using UnityEngine;


public class LightingManager : SingletonDontDestroyOnLoad<LightingManager>{
    [SerializeField] private GameObject _directionalLightObject;
    [SerializeField] private Color _morningDirectionalColor;
    [SerializeField] private float _morningDirectionalIntensity;
    
    [SerializeField] private Color _noonDirectionalColor;
    [SerializeField] private float _noonDirectionalIntensity;
    
    [SerializeField] private Color _eveningDirectionalColor;
    [SerializeField] private float _eveningDirectionalIntensity;

    private MeshRenderer _lanternLeft;
    private MeshRenderer _lanternRight;
    
    [SerializeField] private Material _lanternTurnedOnMaterial;
    [SerializeField] private Material _lanternTurnedOffMaterial;

    [SerializeField] private GameObject _lanternLeftPointLight;
    [SerializeField] private GameObject _lanternRightPointLight;
    
    
  

    private Light _directionalLight;

    public new void Awake(){
        base.Awake();
        _directionalLight = _directionalLightObject.GetComponent<Light>();
        SetMorningLighting();
    }

    private void Start(){
        _lanternLeft = GameObject.Find("LanternLeft").GetComponent<MeshRenderer>();
        _lanternRight = GameObject.Find("LanternRight").GetComponent<MeshRenderer>();
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

    private void ChangeLanternsState(bool turnOn){
        Material[] materials = _lanternLeft.materials;

        if (turnOn){
            materials[6] = _lanternTurnedOnMaterial;
            _lanternLeftPointLight.SetActive(true);
            _lanternRightPointLight.SetActive(true);
        }
        else{
            materials[6] = _lanternTurnedOffMaterial;
            _lanternLeftPointLight.SetActive(false);
            _lanternRightPointLight.SetActive(false);
        }
        
        _lanternLeft.materials = materials;
        _lanternRight.materials = materials;
    }
}
