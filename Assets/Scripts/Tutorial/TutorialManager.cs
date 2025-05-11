using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : SingletonDontDestroyOnLoad<TutorialManager>
{
    private TutorialStateMachine _stateMachine;

    #region Serializable
    [SerializeField] private PlayerDisplayInteraction _playerDisplayInteraction;
    [SerializeField] private DoorPlayerTrigger _guildMerchantTrigger;
    #endregion
    #region Properties
    public PlayerDisplayInteraction PlayerDisplayInteraction => _playerDisplayInteraction;
    public DoorPlayerTrigger DoorPlayerTrigger => _guildMerchantTrigger;
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

        _guildMerchantTrigger.goOutsideEvent.AddListener(TransitionToStepBuyItems);
    }
    private void Update()
    {
        _stateMachine.Update();
    }
    private void TransitionToStepBuyItems()
    {
        _stateMachine.TransitionTo(_stateMachine.state01_Start);
        _guildMerchantTrigger.goOutsideEvent.RemoveListener(TransitionToStepBuyItems);
    }
}
