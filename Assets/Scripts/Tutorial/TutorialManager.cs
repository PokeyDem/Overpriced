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

    #region Tutorial Step Commands
    private TutorialNextStepCommand _state01_Command;
    private TutorialNextStepCommand _state02_Command;
    private TutorialNextStepCommand _state03_Command;
    private TutorialNextStepCommand _state04_Command;
    private TutorialNextStepCommand _state05_Command;
    private TutorialNextStepCommand _state06_Command;
    private TutorialNextStepCommand _state07_Command;
    private TutorialNextStepCommand _state08_Command;
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
    }
    private void Start()
    {
        _stateMachine = new TutorialStateMachine(this);
        _state01_Command = new TutorialNextStepCommand(_merchantGuildTrigger.goOutsideEvent, _stateMachine.state02_GoToShop, _stateMachine);
        _state02_Command = new TutorialNextStepCommand(_merchantGuildManager.buyItemEventTutorial, _stateMachine.state03_BuyItems, _stateMachine);
        _state03_Command = new TutorialNextStepCommand(_merchantGuildExitButton.onClick, _stateMachine.state04_ExitMerchantGuild, _stateMachine);
        _state04_Command = new TutorialNextStepCommand(DisplaysWithItemsListHandler.Instance.itemPlaced, _stateMachine.state05_PutItemOnDisplay, _stateMachine);
        _state05_Command = new TutorialNextStepCommand(OpenShopButton.onClick, _stateMachine.state06_OpenShop, _stateMachine);
        _state06_Command = new TutorialNextStepCommand(NpcReadyToHaggleTrigger.readyToHaggle, _stateMachine.state07_WaitForBuyer, _stateMachine);
        _state07_Command = new TutorialNextStepCommand(HagglingManager.HagglingInitiated, _stateMachine.state08_StartHaggling, _stateMachine);
        _state08_Command = new TutorialNextStepCommand(null, _stateMachine.state01_Start, _stateMachine);

        _state01_Command.NextCommand = _state02_Command;
        _state02_Command.NextCommand = _state03_Command;
        _state03_Command.NextCommand = _state04_Command;
        _state04_Command.NextCommand = _state05_Command;
        _state05_Command.NextCommand = _state06_Command;
        _state06_Command.NextCommand = _state07_Command;
        _state07_Command.NextCommand = _state08_Command;

        _stateMachine.Initialize(_stateMachine.state01_Start);
        _state01_Command.Execute();
    }
    private void Update()
    {
        _stateMachine.Update();
    }
}
