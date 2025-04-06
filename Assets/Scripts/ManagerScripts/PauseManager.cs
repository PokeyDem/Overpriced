using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : SingletonDontDestroyOnLoad<PauseMenuManager>{
    
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _Hud;
    private bool _isPaused;
    
    private new void Awake(){
        base.Awake();
    }
    
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
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
        _Hud.SetActive(false);
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume(){
        if (SaveUIManager.Instance.gameObject.activeInHierarchy)
            SaveUIManager.Instance.DisableUI();
        
        _pauseMenu.SetActive(false);
        _Hud.SetActive(true);
        
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void LoadLevel(string sceneName){
        Resume();
        SceneManager.LoadScene(sceneName);
    }
 
}
