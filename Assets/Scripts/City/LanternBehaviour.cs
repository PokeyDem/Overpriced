using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehaviour : MonoBehaviour, Ilight
{
    [SerializeField] private bool _isTurnedOn;
    [SerializeField] private Material _turnedOnMaterial;
    [SerializeField] private Material _tunedOffMaterial;
    [SerializeField] private int _materialIndex;
    [SerializeField] private bool _isChangingMaterial;
    private MeshRenderer _meshRenderer;
    private Light _lightSource;


    private void Awake()
    {
        _lightSource = gameObject.GetComponentInChildren<Light>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void ChangeState(bool state){
        if (_isChangingMaterial)
        {
                
            Material[] materials = _meshRenderer.materials;
            

            if (state)
                materials[_materialIndex] = _turnedOnMaterial;
            else
                materials[_materialIndex] = _tunedOffMaterial;
        
            _meshRenderer.materials = materials;
        }
        
        if (state)
            TurnOn();
        else
            TurnOff();
        
        _isTurnedOn = state;
    }

    public void TurnOn(){
        _lightSource.gameObject.SetActive(true);
    }

    public void TurnOff(){
        if (_lightSource)
            _lightSource.gameObject.SetActive(false);
        else
            Debug.Log("Light source not found: " + gameObject.name);
    }
}
