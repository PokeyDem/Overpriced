using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour{
    private String _shopCamName = "ShopCamera";
    private String _cityCamName = "CityCamera";
    [SerializeField] private Camera _shopCam;
    [SerializeField] private Camera _cityCam;

    private void Awake(){
        _shopCam.enabled = true;
        _cityCam.enabled = false;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _shopCam.enabled == false){
            _cityCam.enabled = false;
            _shopCam.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && _cityCam.enabled == false){
            _shopCam.enabled = false;
            _cityCam.enabled = true;
        }
    }
}
