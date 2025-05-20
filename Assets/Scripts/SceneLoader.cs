using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonDontDestroyOnLoad<SceneLoader>{
  private String _sceneName;

  public void OnCityEnter(){
    _sceneName = SceneManager.GetActiveScene().name;
    SaveManager.Instance.SaveGameInTemporarySlot();
    SceneManager.LoadScene("CityScene");
  }

  public void OnCityExit(){
    SceneManager.LoadScene(_sceneName);
    SaveManager.Instance.LoadGameFromTemporarySlot();
  }
}
