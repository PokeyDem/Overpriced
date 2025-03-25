using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour{
    
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] private GameObject _mainView;
    private bool _isPaused = false;

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
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume(){
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void LoadLevel(string sceneName){
        Resume();
        SceneManager.LoadScene(sceneName);
    }
 
}
