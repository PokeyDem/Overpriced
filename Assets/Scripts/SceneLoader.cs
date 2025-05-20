using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonDontDestroyOnLoad<SceneLoader>
{
  private Scene _shopScene;
  private GameObject[] _shopSceneGameObjects;

  public void OnCityEnter(){
    _shopScene = SceneManager.GetActiveScene();
    _shopSceneGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
    SceneManager.LoadScene("CityScene");
  }

  public void OnCityExit(){
    SceneManager.LoadScene(_shopScene.name);
  }
}
