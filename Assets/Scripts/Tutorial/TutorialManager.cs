using ManagerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : SingletonDontDestroyOnLoad<TutorialManager>
{
    private TutorialStateMachine _stateMachine;

    #region Serializable
    [Header("Classes")]
    [SerializeField] private PlayerDisplayInteraction _playerDisplayInteraction;
    [SerializeField] private DoorPlayerTrigger _merchantGuildTrigger;
    [SerializeField] private MerchantGuildManager _merchantGuildManager;
    [SerializeField] private Button _merchantGuildBuyButton;
    [SerializeField] private Button _merchantGuildExitButton;
    [SerializeField] private Button _openShopButton;
    [SerializeField] private ReadyCheckerBehaviour _npcReadyToHaggleTrigger;
    [SerializeField] private HagglingManager _hagglingManager;
    #endregion
    #region UITutorialTexts
    [Header("UITutorialTexts")]
    [SerializeField] private GameObject _goToShopText;
    [SerializeField] private GameObject _chooseitemToBuyText;
    [SerializeField] private GameObject _buyItemsText;
    [SerializeField] private GameObject _exitGuildMerchantText;
    [SerializeField] private GameObject _putItemOnDisplayText;
    [SerializeField] private GameObject _openShopText;
    [SerializeField] private GameObject _startHagglingText;
    #endregion
    #region Properties
    public PlayerDisplayInteraction PlayerDisplayInteraction => _playerDisplayInteraction;
    public DoorPlayerTrigger MerchantGuildTrigger => _merchantGuildTrigger;
    public Button MerchantGuildBuyButton => _merchantGuildBuyButton;
    public Button MerchantGuildExitButton => _merchantGuildExitButton;
    public Button OpenShopButton => _openShopButton;
    public ReadyCheckerBehaviour NpcReadyToHaggleTrigger => _npcReadyToHaggleTrigger;
    public HagglingManager HagglingManager => _hagglingManager;


    public GameObject GoToShopText => _goToShopText;
    public GameObject ChooseitemToBuyText => _chooseitemToBuyText;
    public GameObject BuyItemsText => _buyItemsText;
    public GameObject ExitGuildMerchantText => _exitGuildMerchantText;
    public GameObject PutItemOnDisplayText => _putItemOnDisplayText;
    public GameObject OpenShopText => _openShopText;
    public GameObject StartHagglingText => _startHagglingText;
    #endregion
    private new void Awake()
    {
        base.Awake();
        _stateMachine = new TutorialStateMachine(this);
    }
    private void Start()
    {
        _stateMachine.Initialize(_stateMachine.state01_Start);
        _stateMachine.TransitionTo(_stateMachine.state02_GoToShop);//pointless

        _merchantGuildTrigger.goOutsideEvent.AddListener(TransitionToStepBuyItems);
    }
    private void Update()
    {
        _stateMachine.Update();
    }

    private void TransitionToStepBuyItems()
    {
        _merchantGuildTrigger.goOutsideEvent.RemoveListener(TransitionToStepBuyItems);
        _stateMachine.TransitionTo(_stateMachine.state03_BuyItems);
        _merchantGuildManager.buyItemEvent.AddListener(TransitionToStepExitMerchantGuild);
    }
    private void TransitionToStepExitMerchantGuild(ItemData item)
    {
        _merchantGuildManager.buyItemEvent.RemoveListener(TransitionToStepExitMerchantGuild);
        _stateMachine.TransitionTo(_stateMachine.state04_ExitMerchantGuild);
        _merchantGuildExitButton.onClick.AddListener(TransitionToStepPutItemOnDisplay);
    }
    private void TransitionToStepPutItemOnDisplay()
    {
        _merchantGuildExitButton.onClick.RemoveListener(TransitionToStepPutItemOnDisplay);
        _stateMachine.TransitionTo(_stateMachine.state05_PutItemOnDisplay);
        DisplaysWithItemsListHandler.Instance.itemPlaced.AddListener(TransitionToStepOpenShop);
    }
    private void TransitionToStepOpenShop()
    {
        DisplaysWithItemsListHandler.Instance.itemPlaced.RemoveListener(TransitionToStepOpenShop);
        _stateMachine.TransitionTo(_stateMachine.state06_OpenShop);
        OpenShopButton.onClick.AddListener(TransitionToStepWaitForBuyer);
    }
    private void TransitionToStepWaitForBuyer()
    {
        DisplaysWithItemsListHandler.Instance.itemPlaced.RemoveListener(TransitionToStepOpenShop);
        _stateMachine.TransitionTo(_stateMachine.state07_WaitForBuyer);
        NpcReadyToHaggleTrigger.readyToHaggle.AddListener(TransitionToStepStartHaggling);
    }
    private void TransitionToStepStartHaggling()
    {
        NpcReadyToHaggleTrigger.readyToHaggle.RemoveListener(TransitionToStepStartHaggling);
        _stateMachine.TransitionTo(_stateMachine.state08_StartHaggling);
        HagglingManager.HagglingInitiated += TransitionToStepEnd;
    }
    private void TransitionToStepEnd()
    {
        HagglingManager.HagglingInitiated -= TransitionToStepEnd;
        _stateMachine.TransitionTo(_stateMachine.state01_Start);
    }
}
