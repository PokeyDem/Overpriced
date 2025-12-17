
using System;
using System.Collections;
using System.Collections.Generic;
using ManagerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroyOnLoad<GameManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 initialPlayerPosition;
    [SerializeField] private Quaternion initialPlayerRotation;
    [SerializeField] private float _goalAmount;
    public TutorialManager tutorialManager;
    private bool hasWon;
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
        hasWon = MoneyManager.Instance.GetCurrentMoney() >= _goalAmount;
        if (day > 7)
        {
            EndSceneManager.Instance.StartEndScene();
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

    public bool HasWon()
    {
        return hasWon;
    }
    
}
