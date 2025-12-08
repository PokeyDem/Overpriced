using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : SingletonDontDestroyOnLoad<PauseMenuManager>{
    
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _Hud;
    private bool _isPaused;
    private bool _canPause = true;
    
    private new void Awake(){
        base.Awake();
    }
    
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) && _canPause && ShopStateManager.ShopStateManagerInstance.ShopIsClose()){
            EscPressed();
        } 
    }

    public void EscPressed(){
        if (_isPaused)
            Resume();
        else{
            Pause();
        }
    }

    public void Pause(){
        // _Hud.SetActive(false);
        // _pauseMenu.SetActive(true);
        // Time.timeScale = 0f;
        MainMenuManager.Instance.SwitchToMainMenu();
        _isPaused = true;
    }

    public void Resume(){
        // if (SaveUIManager.Instance.gameObject.activeInHierarchy)
        //     SaveUIManager.Instance.DisableUI();
        //
        // _pauseMenu.SetActive(false);
        // _Hud.SetActive(true);
        //
        // Time.timeScale = 1f;
        MainMenuManager.Instance.SwitchToGame();
        _isPaused = false;
    }

    public void EnablePausing()
    {
        _canPause = true;
    }

    public void DisablePausing()
    {
        _canPause = false;
    }
}
