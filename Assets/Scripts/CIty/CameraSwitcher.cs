using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CameraSwitcher : SingletonDontDestroyOnLoad<CameraSwitcher>{
    private String _shopCamName = "ShopCamera";
    private String _cityCamName = "CityCamera";
    [SerializeField] private Camera _shopCam;
    [SerializeField] private Camera _cityCam;

    private new void Awake(){
        base.Awake();
        if (SceneManager.loadedSceneCount  < 2) 
            SceneManager.LoadScene("New_City_scene", LoadSceneMode.Additive);
        _shopCam.enabled = true;
        _cityCam.enabled = false;
    }

    public void SwitchToShopCam(){
        _shopCam.enabled = true;
        _cityCam.enabled = false;
        PlayerControl.Instance.EnableControl();
        PauseMenuManager.Instance.EnablePausing();
    }

    public void SwitchToCityCam(){
        _cityCam.enabled = true;
        _shopCam.enabled = false;
        PlayerControl.Instance.DisableControl();
        PauseMenuManager.Instance.DisablePausing();
    }
}
