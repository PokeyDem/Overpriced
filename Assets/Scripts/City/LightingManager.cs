using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightingManager : SingletonDontDestroyOnLoad<LightingManager>{
    [Header("General light sources")]
    [SerializeField] private GameObject directionalLightObject;
    [SerializeField] private Light seilingFrontLight;
    [SerializeField] private Light seilingBackLight;
    [SerializeField] private GameObject[] windowPointLights;
    [SerializeField] private float duration;

    [SerializeField] private Light spotLightWindow;
    [SerializeField] private Light spotLightDoor;
    
    [Header("Morning lighting")]
    [SerializeField] private Color morningDirectionalColor;
    [SerializeField] private float morningDirectionalIntensity;
    [SerializeField] private float spotLightDoorIntensity;
    [SerializeField] private float spotLightWindowIntensity;
    private float seilingFrontMorningLightIntensity;
    private float seilingBackMorningLightIntensity;
    
    [Header("Noon lighting")]
    [SerializeField] private Color noonDirectionalColor;
    [SerializeField] private float noonDirectionalIntensity;
    [SerializeField] private float seilingFrontNoonLightIntensity;
    [SerializeField] private float seilingBackNoonLightIntensity;
    
    [Header("Evening lighting")]
    [SerializeField] private Color eveningDirectionalColor;
    [SerializeField] private float eveningDirectionalIntensity;
    [SerializeField] private float seilingFrontEveningLightIntensity;
    [SerializeField] private float seilingBackEveningLightIntensity;

    [Header("Switchable light sources")]
    [SerializeField] private List<LanternBehaviour> lanterns = new List<LanternBehaviour>();
    
    [SerializeField] private Material lanternTurnedOnMaterial;
    [SerializeField] private Material lanternTurnedOffMaterial;
    
    [SerializeField] private List<MeshRenderer> buildings = new List<MeshRenderer>();
    [SerializeField] private Material windowsDefaultMaterial;
    [SerializeField] private Material windowsWithLightOnMaterial;

    [Header("Fireplace")] 
    [SerializeField] private Light _fireplaceLight;
    [SerializeField] ParticleSystem _fireplaceParticles;
    [SerializeField] FireplaceLightDistortion _fireplaceLightDistortion;
    private float fireplaceIntensity;
    
    private Light _directionalLight;
    private DayManager.PartOfDay _partOfDayLighting;
    private float _spotLightWindowIntensity;
    private float _spotLightDoorIntensity;
    
    public new void Awake(){
        base.Awake();
        
        _directionalLight = directionalLightObject.GetComponent<Light>();
        buildings = new List<MeshRenderer>();
    }

    private void Start()
    {
        seilingFrontMorningLightIntensity = seilingFrontLight.intensity;
        seilingBackMorningLightIntensity = seilingBackLight.intensity;
        fireplaceIntensity = _fireplaceLight.intensity;
        
        _spotLightWindowIntensity = spotLightWindow.intensity;
        _spotLightDoorIntensity = spotLightDoor.intensity;
        
        TurnOffFireplace();
        
        foreach (var lantern in GameObject.FindGameObjectsWithTag("Lantern")){
            lanterns.Add(lantern.GetComponent<LanternBehaviour>());
        }
        
        foreach (var building in GameObject.FindGameObjectsWithTag("CityBuilding")){
            Debug.Log(building.name);
            if (building.TryGetComponent(out MeshRenderer meshRenderer)){
                Debug.Log("MeshRenderer found");
                buildings.Add(meshRenderer);
            }
            else
                Debug.Log("Mesh renderer not found");
        }
        SetMorningLighting();
    }

    private void TurnOffFireplace()
    {
        _fireplaceParticles.Stop();
        StartCoroutine(SmoothIntensityTransition(_fireplaceLight, 0, 0.5f));
        _fireplaceLightDistortion.ChangeLanternsState(false);
    }

    private void TurnOnFireplace()
    {
        _fireplaceParticles.Play();
        StartCoroutine(SmoothIntensityTransition(_fireplaceLight, fireplaceIntensity, 0.5f));
        _fireplaceLightDistortion.ChangeLanternsState(true);
    }
    

    private void TurnOnSpotLights()
    {
        StartCoroutine(SmoothIntensityTransition(spotLightWindow, _spotLightWindowIntensity, 0));
        StartCoroutine(SmoothIntensityTransition(spotLightDoor, _spotLightDoorIntensity, 0));
    }

    private void TurnOffSpotLights()
    {
        StartCoroutine(SmoothIntensityTransition(seilingBackLight, 0, 0));
        StartCoroutine(SmoothIntensityTransition(seilingFrontLight, 0, 0));
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
        TurnOffFireplace();
       TurnOnSpotLights();
       StartCoroutine(SmoothIntensityTransition(_directionalLight, morningDirectionalIntensity, 0));
       StartCoroutine(SmoothColorTransition(_directionalLight, morningDirectionalColor));

       StartCoroutine(SmoothIntensityTransition(seilingBackLight, seilingBackMorningLightIntensity, 0));
       StartCoroutine(SmoothIntensityTransition(seilingFrontLight, seilingFrontMorningLightIntensity, 0));
       
       StartCoroutine(SmoothIntensityTransition(spotLightDoor, spotLightDoorIntensity, 0));
       StartCoroutine(SmoothIntensityTransition(spotLightWindow, spotLightWindowIntensity, 0));
       
       ChangeLanternsState(false);
       ChangeBuildingWindowsLightState(false);
    }

    private IEnumerator SmoothIntensityTransition(Light targetLight, float targetIntensity, float setDuration)
    {
        float elapsed = 0f;
        float initialIntensity = targetLight.intensity;
        float tempIntensity = initialIntensity;

        float customDuration = duration;
        
        if (setDuration != 0)
            customDuration = setDuration;
            

        while (elapsed < customDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / customDuration;
            t = Mathf.SmoothStep(0,1,t);
            tempIntensity = Mathf.Lerp(initialIntensity, targetIntensity, t);
            targetLight.intensity = tempIntensity;
            yield return null;
        }
        
        targetLight.intensity = targetIntensity;
    }
    
    private IEnumerator SmoothColorTransition(Light targetLight, Color targetColor)
    {
        float elapsed = 0f;
        Color initialColor = targetLight.color;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0,1,t);
            targetLight.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }
        
        targetLight.color = targetColor;
    }
    

    private void SetNoonLighting(){
        StartCoroutine(SmoothIntensityTransition(_directionalLight, noonDirectionalIntensity, 0));
        StartCoroutine(SmoothColorTransition(_directionalLight, noonDirectionalColor));
        
        StartCoroutine(SmoothIntensityTransition(seilingBackLight, seilingBackNoonLightIntensity, 0));
        StartCoroutine(SmoothIntensityTransition(seilingFrontLight, seilingFrontNoonLightIntensity, 0));
        
        StartCoroutine(SmoothIntensityTransition(spotLightDoor, spotLightDoor.intensity * 0.5f, 0));
        StartCoroutine(SmoothIntensityTransition(spotLightWindow, spotLightWindow.intensity * 0.5f, 0));
    }

    private void SetEveningLighting(){
        TurnOnFireplace();
       TurnOffSpotLights();
       StartCoroutine(SmoothIntensityTransition(_directionalLight, eveningDirectionalIntensity, 0));
       StartCoroutine(SmoothColorTransition(_directionalLight, eveningDirectionalColor));
       
       StartCoroutine(SmoothIntensityTransition(seilingBackLight, seilingBackEveningLightIntensity, 0));
       StartCoroutine(SmoothIntensityTransition(seilingFrontLight, seilingFrontEveningLightIntensity, 0));
       
       StartCoroutine(SmoothIntensityTransition(spotLightDoor, spotLightDoor.intensity * 0.5f, 0));
       StartCoroutine(SmoothIntensityTransition(spotLightWindow, spotLightWindow.intensity * 0.5f, 0));
       
       ChangeLanternsState(true);
       ChangeBuildingWindowsLightState(true);
    }

    private void ChangeLanternsState(bool turnOn){
        foreach (var lantern in lanterns){
            lantern.ChangeState(turnOn); 
        }
    }

    private void ChangeBuildingWindowsLightState(bool turnOn){
        foreach (var building in buildings){
            Material[] materials = building.materials;

            if (turnOn){
                if (building.name == "Building_3")
                    materials[4] = windowsWithLightOnMaterial;
                else
                    materials[3] = windowsWithLightOnMaterial;
            }
            else{
                if (building.name == "Building_3")
                    materials[4] = windowsDefaultMaterial;
                else 
                    materials[3] = windowsDefaultMaterial;
            }
            
            building.materials = materials;
            foreach (var windowPointLight in windowPointLights){
                windowPointLight.SetActive(turnOn);
            }
        }
    }
}
