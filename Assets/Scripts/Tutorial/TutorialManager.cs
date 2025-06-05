using ManagerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : SingletonWithDestroy<TutorialManager>
{
    private TutorialStateMachine _stateMachine;
    [SerializeField] private bool _tutorialDone = true;
    private TutorialNextStepCommand _currentCommand;

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
    [SerializeField] private Button _sellButton;
    #endregion
    #region UITutorialTexts
    [Header("UITutorialTexts")]
    [SerializeField] private GameObject _goOutsideText;
    [SerializeField] private GameObject _goToGuildText;
    [SerializeField] private GameObject _chooseitemToBuyText;
    [SerializeField] private GameObject _buyItemsText;
    [SerializeField] private GameObject _exitGuildMerchantText;
    [SerializeField] private GameObject _returnToShopText;
    [SerializeField] private GameObject _putItemOnDisplayText;
    [SerializeField] private GameObject _openShopText;
    [SerializeField] private GameObject _startHagglingText;
    [SerializeField] private GameObject _changePriceValueText;
    [SerializeField] private GameObject _tryToSellText;
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
    private TutorialNextStepCommand _state09_Command;
    private TutorialNextStepCommand _state10_Command;
    private TutorialNextStepCommand _state11_Command;
    private TutorialNextStepCommand _state12_Command;
    #endregion
    #region Properties
    public TutorialStateMachine StateMachine => _stateMachine;
    public bool TutorialDone => _tutorialDone;
    public PlayerDisplayInteraction PlayerDisplayInteraction => _playerDisplayInteraction;
    public DoorPlayerTrigger MerchantGuildTrigger => _merchantGuildTrigger;
    public Button MerchantGuildBuyButton => _merchantGuildBuyButton;
    public Button MerchantGuildExitButton => _merchantGuildExitButton;
    public Button OpenShopButton => _openShopButton;
    public ReadyCheckerBehaviour NpcReadyToHaggleTrigger => _npcReadyToHaggleTrigger;
    public HagglingManager HagglingManager => _hagglingManager;
    public Button SellButton => _sellButton;


    public GameObject GoOutsideText => _goOutsideText;
    public GameObject GoToGuildText => _goToGuildText;
    public GameObject ChooseitemToBuyText => _chooseitemToBuyText;
    public GameObject BuyItemsText => _buyItemsText;
    public GameObject ExitGuildMerchantText => _exitGuildMerchantText;
    public GameObject ReturnToShopText => _returnToShopText;
    public GameObject PutItemOnDisplayText => _putItemOnDisplayText;
    public GameObject OpenShopText => _openShopText;
    public GameObject StartHagglingText => _startHagglingText;
    public GameObject ChangePriceValueText => _changePriceValueText;
    public GameObject TryToSellText => _tryToSellText;
    #endregion
    private new void Awake()
    {
        base.Awake();
        _stateMachine = new TutorialStateMachine(this);
        _stateMachine.Initialize(_stateMachine.stateNothing);
    }
    private void Start()
    {
        GameManager.Instance.tutorialManager=this;
        _state01_Command = new TutorialNextStepCommand(null, _merchantGuildTrigger.goOutsideEvent, _stateMachine.stateGoToShop, _stateMachine);
        _state02_Command = new TutorialNextStepCommand(_state01_Command, _merchantGuildManager.buyItemEventTutorial, _stateMachine.stateBuyItems, _stateMachine);
        _state03_Command = new TutorialNextStepCommand(_state02_Command, _merchantGuildExitButton.onClick, _stateMachine.stateExitMerchantGuild, _stateMachine);
        _state04_Command = new TutorialNextStepCommand(_state03_Command, DisplaysWithItemsListHandler.Instance.itemPlaced, _stateMachine.statePutItemOnDisplay, _stateMachine);
        _state05_Command = new TutorialNextStepCommand(_state04_Command, OpenShopButton.onClick, _stateMachine.stateOpenShop, _stateMachine);
        _state06_Command = new TutorialNextStepCommand(_state05_Command, NpcReadyToHaggleTrigger.readyToHaggle, _stateMachine.stateWaitForBuyer, _stateMachine);
        _state07_Command = new TutorialNextStepCommand(_state06_Command, HagglingManager.HagglingInitiated, _stateMachine.stateStartHaggling, _stateMachine);
        _state08_Command = new TutorialNextStepCommand(_state07_Command, HagglingManager.PriceChanged, _stateMachine.stateChangePriceValue, _stateMachine);
        _state09_Command = new TutorialNextStepCommand(_state08_Command, SellButton.onClick, _stateMachine.stateTrySell, _stateMachine);
        _state10_Command = new TutorialNextStepCommand(_state09_Command, null, _stateMachine.stateEnd, _stateMachine);

        Initialize();
    }
    private void Update()
    {
        _stateMachine.Update();
    }
    public void Initialize()
    {
        _tutorialDone = false;
        _state01_Command.Execute();
    }
    public void DisableTutorial()
    {
        _tutorialDone=true;
        this.gameObject.SetActive(false);
    }
    public void SetCurrentCommand(TutorialNextStepCommand command)
    {
        _currentCommand.RemoveNextListener();
        _currentCommand = command;
        _currentCommand.Execute();
    }
}
