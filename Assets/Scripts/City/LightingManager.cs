using System;
using System.Collections.Generic;
using UnityEngine;


public class LightingManager : SingletonDontDestroyOnLoad<LightingManager>{
    [Header("General light sources")]
    [SerializeField] private GameObject _directionalLightObject;
    [SerializeField] private Light _seilingFrontLight;
    [SerializeField] private Light _seilingBackLight;
    [SerializeField] private GameObject[] _windowPointLights;

    [SerializeField] private Light _spotLightWindow;
    [SerializeField] private Light _spotLightDoor;
    
    [Header("Morning lighting")]
    [SerializeField] private Color _morningDirectionalColor;
    [SerializeField] private float _morningDirectionalIntensity;
    
    [Header("Noon lighting")]
    [SerializeField] private Color _noonDirectionalColor;
    [SerializeField] private float _noonDirectionalIntensity;
    
    [Header("Evening lighting")]
    [SerializeField] private Color _eveningDirectionalColor;
    [SerializeField] private float _eveningDirectionalIntensity;

    [Header("Switchable light sources")]
    [SerializeField] private List<LanternBehaviour> _lanterns = new List<LanternBehaviour>();
    
    [SerializeField] private Material _lanternTurnedOnMaterial;
    [SerializeField] private Material _lanternTurnedOffMaterial;
    
    [SerializeField] private List<MeshRenderer> _buildings = new List<MeshRenderer>();
    [SerializeField] private Material _windowsDefaultMaterial;
    [SerializeField] private Material _windowsWithLightOnMaterial;
    
    private Light _directionalLight;
    private DayManager.PartOfDay _partOfDayLighting;

    public new void Awake(){
        base.Awake();
        
        _directionalLight = _directionalLightObject.GetComponent<Light>();
        _buildings = new List<MeshRenderer>();
    }

    private void Start(){
        
        foreach (var lantern in GameObject.FindGameObjectsWithTag("Lantern")){
            _lanterns.Add(lantern.GetComponent<LanternBehaviour>());
        }
        
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_partOfDayLighting == DayManager.PartOfDay.Evening)
            {
                SetMorningLighting();
                _partOfDayLighting = DayManager.PartOfDay.Morning;
            }else if (_partOfDayLighting == DayManager.PartOfDay.Morning)
            {
                SetNoonLighting();
                _partOfDayLighting = DayManager.PartOfDay.Noon;
            }else if (_partOfDayLighting == DayManager.PartOfDay.Noon)
            {
                SetEveningLighting();
                _partOfDayLighting = DayManager.PartOfDay.Evening;
            }
        }
    }

    private void SetMorningLighting(){
       _directionalLight.color = _morningDirectionalColor;
       _directionalLight.intensity = _morningDirectionalIntensity;
       
       _seilingBackLight.gameObject.SetActive(true);
       _seilingFrontLight.gameObject.SetActive(true);
       _spotLightWindow.gameObject.SetActive(true);
       _spotLightDoor.gameObject.SetActive(true);
       
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
       
       _seilingBackLight.gameObject.SetActive(false);
       _seilingFrontLight.gameObject.SetActive(false);
       _spotLightWindow.gameObject.SetActive(false);
       _spotLightDoor.gameObject.SetActive(false);
       
       ChangeLanternsState(true);
       ChangeBuildingsWindowsLightState(true);
    }

    private void ChangeLanternsState(bool turnOn){
        foreach (var lantern in _lanterns){
            lantern.ChangeState(turnOn); 
        }
    }

    private void ChangeBuildingsWindowsLightState(bool turnOn){
        foreach (var building in _buildings){
            Material[] materials = building.materials;

            if (turnOn){
                if (building.name == "Building_3")
                    materials[4] = _windowsWithLightOnMaterial;
                else
                    materials[3] = _windowsWithLightOnMaterial;
            }
            else{
                if (building.name == "Building_3")
                    materials[4] = _windowsDefaultMaterial;
                else 
                    materials[3] = _windowsDefaultMaterial;
            }
            
            building.materials = materials;
            foreach (var windowPointLight in _windowPointLights){
                windowPointLight.SetActive(turnOn);
            }
        }
    }
}
