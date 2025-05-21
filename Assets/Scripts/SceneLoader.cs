using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonDontDestroyOnLoad<SceneLoader>{

  private Scene _shop;
  private string _shopSceneName = "Aleksandr_dev_scene";
  private Scene _city;
  private string _citySceneName = "City_scene";
  

  private new void Awake(){
    base.Awake();
  }

  private void Start(){
    SceneManager.LoadScene("PersistentScene", LoadSceneMode.Additive);
  }

  public void OnCityEnter(){
    SaveManager.Instance.SaveGameInTemporarySlot();
    SceneManager.LoadScene("City_scene");
  }

  public void OnCityExit(){
    Debug.Log("Invoked");
    SceneManager.LoadScene("Aleksandr_dev_scene");
    SaveManager.Instance.LoadGameFromTemporarySlot();
    foreach (var boughtItem in DataTransfer.Instance.GetBoughtItems()){
      InventoryManager.Instance.AddItemToInventory(boughtItem);
    }
  }
}
