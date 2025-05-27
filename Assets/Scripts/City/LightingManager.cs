using System;
using System.Collections.Generic;
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
    
    private List<MeshRenderer> _buildings = new List<MeshRenderer>();
    [SerializeField] private Material _windowsDefaultMaterial;
    [SerializeField] private Material _windowsWithLightOnMaterial;
    [SerializeField] private GameObject[] _windowPointLights;
    
    
  

    private Light _directionalLight;

    public new void Awake(){
        base.Awake();
        _directionalLight = _directionalLightObject.GetComponent<Light>();
        _buildings = new List<MeshRenderer>();
    }

    private void Start(){
        _lanternLeft = GameObject.Find("LanternLeft").GetComponent<MeshRenderer>();
        _lanternRight = GameObject.Find("LanternRight").GetComponent<MeshRenderer>();
        foreach (var building in GameObject.FindGameObjectsWithTag("CityBuilding")){
            Debug.Log(building.name);
            if (building.TryGetComponent(out MeshRenderer meshRenderer)){
                Debug.Log("MeshRenderer found");
                _buildings.Add(meshRenderer);
            }
            else
                Debug.Log("Mesh renderer not found");
        }
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
       ChangeLanternsState(false);
       ChangeBuildingsWindowsLightState(false);
    }

    private void SetNoonLighting(){
        _directionalLight.color = _noonDirectionalColor;
        _directionalLight.intensity = _noonDirectionalIntensity;
    }

    private void SetEveningLighting(){
       _directionalLight.color = _eveningDirectionalColor;
       _directionalLight.intensity = _eveningDirectionalIntensity;
       ChangeLanternsState(true);
       ChangeBuildingsWindowsLightState(true);
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

    private void ChangeBuildingsWindowsLightState(bool turnOn){
        foreach (var building in _buildings){
            Material[] materials = building.materials;

            if (turnOn){
                materials[3] = _windowsWithLightOnMaterial;
            }
            else
                materials[3] = _windowsDefaultMaterial;
            
            building.materials = materials;
            foreach (var windowPointLight in _windowPointLights){
                windowPointLight.SetActive(turnOn);
            }
        }
    }
}
