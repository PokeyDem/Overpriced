
using System;
using System.Collections;
using System.Collections.Generic;
using ManagerScripts;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroyOnLoad<GameManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 initialPlayerPosition;
    [SerializeField] private Quaternion initialPlayerRotation;
    public TutorialManager tutorialManager;
    private new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        DayManager.Instance.dayChange.AddListener(End);
        MainMenuManager.Instance.SwitchToMainMenu();
    }
    private void End(int day)
    {
        if (day > 7)
        {
            SceneManager.LoadScene("EndScene");
        }
    }


    public void ReloadGame()
    {
        MoneyManager.Instance.SetMoney(500);
        ExperienceManager.ExperienceManagerInstance.ResetLevel();
        InventoryManager.Instance.ClearInventory();
        MerchantGuildManager.Instance.ResetShop();
        DisplaysManager.Instance.ResetDisplays();
        DayManager.Instance.Reset();
        LightingManager.Instance.SetLighting(DayManager.Instance.GetPartOfDay());
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        player.transform.position =  initialPlayerPosition;
        player.transform.rotation = initialPlayerRotation;
    }
    
}
