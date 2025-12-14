using ManagerScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private DoorPlayerTrigger _goOutsideTrigger;
    [SerializeField] private PlayerOnClickInteraction _playerOnClickInteraction;
    [SerializeField] private Button _goBackToShopButton;
    [SerializeField] private MerchantGuildManager _merchantGuildManager;
    [SerializeField] private Button _merchantGuildBuyButton;
    [SerializeField] private Button _merchantGuildExitButton;
    [SerializeField] private Button _openShopButton;
    [SerializeField] private ReadyCheckerBehaviour _npcReadyToHaggleTrigger;
    [SerializeField] private HagglingManager _hagglingManager;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Slider _amountSlider;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private DayManager _dayManager;
    [SerializeField] private Button _talentButton;
    #endregion
    #region UITutorialTexts
    [Header("UITutorialTexts")]
    [SerializeField] private GameObject _goOutsideText;
    [SerializeField] private GameObject _dayPhaseConsumeText;
    [SerializeField] private GameObject _dayPhaseConsumeText2;
    [SerializeField] private GameObject _goToGuildText;
    [SerializeField] private GameObject _chooseitemToBuyText;
    [SerializeField] private GameObject _buyItemsText;
    [SerializeField] private GameObject _spendMoneyText;
    [SerializeField] private GameObject _spendMoneyText2;
    [SerializeField] private GameObject _exitGuildMerchantText;
    [SerializeField] private GameObject _returnToShopText;
    [SerializeField] private GameObject _putItemOnDisplayText;
    [SerializeField] private GameObject _openShopText;
    [SerializeField] private GameObject _dayPhaseConsumeOpenShop;
    [SerializeField] private GameObject _npcTypeInfoText;
    [SerializeField] private GameObject _startHagglingText;
    [SerializeField] private GameObject _changePriceValueText;
    [SerializeField] private GameObject _changePriceValueText2;
    [SerializeField] private GameObject _toleranceText1;
    [SerializeField] private GameObject _toleranceText2;
    [SerializeField] private GameObject _chancesLeftText;
    [SerializeField] private GameObject _hintText1;
    [SerializeField] private GameObject _hintText2;
    [SerializeField] private GameObject _tryToSellText;
    [SerializeField] private GameObject _buyDisplayText;
    [SerializeField] private GameObject _talentText;
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
    private TutorialNextStepCommand _state13_Command;
    private TutorialNextStepCommand _state14_Command;
    private TutorialNextStepCommand _state15_Command;
    private TutorialNextStepCommand _state16_Command;
    private TutorialNextStepCommand _state17_Command;
    private TutorialNextStepCommand _state18_Command;
    #endregion
    #region Properties
    public TutorialStateMachine StateMachine => _stateMachine;
    public bool TutorialDone => _tutorialDone;
    public PlayerDisplayInteraction PlayerDisplayInteraction => _playerDisplayInteraction;
    public DoorPlayerTrigger GoOutsideTrigger => _goOutsideTrigger;
    public PlayerOnClickInteraction PlayerOnClickInteraction => _playerOnClickInteraction;
    public Button GoBackToShopButton => _goBackToShopButton;
    public Button MerchantGuildBuyButton => _merchantGuildBuyButton;
    public Button MerchantGuildExitButton => _merchantGuildExitButton;
    public Button OpenShopButton => _openShopButton;
    public ReadyCheckerBehaviour NpcReadyToHaggleTrigger => _npcReadyToHaggleTrigger;
    public HagglingManager HagglingManager => _hagglingManager;
    public Button SellButton => _sellButton;
    public Slider AmountSlider => _amountSlider;
    public TMP_InputField InputField => _inputField;
    public DayManager DayManager => _dayManager;
    public Button TalentButton => _talentButton;


    public GameObject GoOutsideText => _goOutsideText;
    public GameObject DayPhaseConsumeText => _dayPhaseConsumeText;
    public GameObject DayPhaseConsumeText2 => _dayPhaseConsumeText2;
    public GameObject GoToGuildText => _goToGuildText;
    public GameObject ChooseitemToBuyText => _chooseitemToBuyText;
    public GameObject BuyItemsText => _buyItemsText;
    public GameObject SpendMoneyText => _spendMoneyText;
    public GameObject SpendMoneyText2 => _spendMoneyText2;
    public GameObject ExitGuildMerchantText => _exitGuildMerchantText;
    public GameObject ReturnToShopText => _returnToShopText;
    public GameObject PutItemOnDisplayText => _putItemOnDisplayText;
    public GameObject OpenShopText => _openShopText;
    public GameObject DayPhaseConsumeOpenShop => _dayPhaseConsumeOpenShop;
    public GameObject NpcTypeInfoText => _npcTypeInfoText;
    public GameObject StartHagglingText => _startHagglingText;
    public GameObject ChangePriceValueText => _changePriceValueText;
    public GameObject ChangePriceValueText2 => _changePriceValueText2;
    public GameObject ToleranceText1 => _toleranceText1;
    public GameObject ToleranceText2 => _toleranceText2;
    public GameObject ChancesLeftText => _chancesLeftText;
    public GameObject HintText1 => _hintText1;
    public GameObject HintText2 => _hintText2;
    public GameObject TryToSellText => _tryToSellText;
    public GameObject BuyDisplayText => _buyDisplayText;
    public GameObject TalentText => _talentText;
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
        _state01_Command = new TutorialNextStepCommand(null, _goOutsideTrigger.goOutsideEvent, _stateMachine.stateGoToShop, _stateMachine);
        _state02_Command = new TutorialNextStepCommand(_state01_Command, _playerOnClickInteraction.MouseInteraction, _stateMachine.stateEnterMerchantGuild, _stateMachine);
        _state03_Command = new TutorialNextStepCommand(_state02_Command, _merchantGuildManager.buyItemEventTutorial, _stateMachine.stateBuyItems, _stateMachine);
        _state04_Command = new TutorialNextStepCommand(_state03_Command, _merchantGuildManager.allMoneySpentTutorial, _stateMachine.stateSpendAllMoney, _stateMachine);
        _state05_Command = new TutorialNextStepCommand(_state04_Command, _merchantGuildExitButton.onClick, _stateMachine.stateExitMerchantGuild, _stateMachine);
        _state06_Command = new TutorialNextStepCommand(_state05_Command, _goBackToShopButton.onClick, _stateMachine.stateEnterShop, _stateMachine);
        _state07_Command = new TutorialNextStepCommand(_state06_Command, DisplaysWithItemsListHandler.Instance.itemPlaced, _stateMachine.statePutItemOnDisplay, _stateMachine);
        _state08_Command = new TutorialNextStepCommand(_state07_Command, OpenShopButton.onClick, _stateMachine.stateOpenShop, _stateMachine);
        _state09_Command = new TutorialNextStepCommand(_state08_Command, NpcReadyToHaggleTrigger.readyToHaggle, _stateMachine.stateNpcTypeInfo, _stateMachine);
        _state10_Command = new TutorialNextStepCommand(_state09_Command, HagglingManager.HagglingInitiated, _stateMachine.stateStartHaggling, _stateMachine);
        _state11_Command = new TutorialNextStepCommand(_state10_Command, HagglingManager.MarkupTutorial, _stateMachine.stateChangePriceValue, _stateMachine);
        _state12_Command = new TutorialNextStepCommand(_state11_Command, SellButton.onClick, _stateMachine.stateTrySell, _stateMachine);
        _state13_Command = new TutorialNextStepCommand(_state12_Command, SellButton.onClick, _stateMachine.stateToleranceAndHintsInfo, _stateMachine);
        _state14_Command = new TutorialNextStepCommand(_state13_Command, DayManager.partOfDayChangeTutorial, _stateMachine.stateNothing, _stateMachine);
        _state15_Command = new TutorialNextStepCommand(_state14_Command, TalentButton.onClick, _stateMachine.stateTalentsInfo, _stateMachine);
        _state16_Command = new TutorialNextStepCommand(_state15_Command, OpenShopButton.onClick, _stateMachine.stateBuyDisplays, _stateMachine);
        _state17_Command = new TutorialNextStepCommand(_state16_Command, null, _stateMachine.stateEnd, _stateMachine);

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

    public void EnableTutorial()
    {
        _tutorialDone=false;
        gameObject.SetActive(true);
    }
    public void SetCurrentCommand(TutorialNextStepCommand command)
    {
        _currentCommand.RemoveNextListener();
        _currentCommand = command;
        _currentCommand.Execute();
    }
}
