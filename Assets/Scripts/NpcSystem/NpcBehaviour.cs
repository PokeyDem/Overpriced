using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public class NpcBehaviour : MonoBehaviour{

    [SerializeField] private int _tolerance;
    [SerializeField] private NPCType _NPCType;
    private NavMeshAgent _agent;
    [SerializeField] private ItemData _itemToBuy; //Set by ChooseItem
    private Transform _despawnPointPos;
    private Transform _windowPos; 
    private Transform _doorPos;
    private Transform _despawnInShop;
    private DisplaySlotController _displaySlotController;
    private Transform _counterPos;

    [SerializeField] private bool _isInShop;
    [SerializeField] private NPCDesiredItemsSO _desiredItemsSO;

    private void Awake()
    {
    }

    public void Initialize(bool isInShop, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, DisplaySlotController displaySlotController, Transform counterPos){
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos;
        _doorPos = doorPos;
        _displaySlotController = displaySlotController;
        _isInShop = isInShop;
        _counterPos = counterPos;
        _despawnInShop = despawnInShop;
    }

    private void Start(){
        _agent = GetComponent<NavMeshAgent>();
        if (!_isInShop){
            _agent.SetDestination(_windowPos.position);
            StartCoroutine(LookingDelay());
        }
        else{
            Debug.Log("Spawned in shop");
            StartCoroutine(BrowseDisplays());
        }
    }


    private IEnumerator LookingDelay()
    {
        yield return new WaitForSeconds(4);
        if (DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count != 0)
        {
            _agent.SetDestination(_doorPos.position);
        }
        else
        {
            _agent.SetDestination(_despawnPointPos.position);
            Debug.Log("Item wasn't found, Npc destination: DespawnPoint");
        }
    }

    public IEnumerator BrowseDisplays()
    {
        Debug.Log($"BrowseDisplays started");

        foreach (var displaySlot in DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems())
        {
            if (displaySlot==null) {
                continue;
            }
            DisplaySlotController displaySlotController = displaySlot;

            ItemData item = displaySlotController.GetItem();

            _agent.speed = 2;
            _agent.SetDestination(displaySlot.transform.position);
            Debug.Log($"Going to: {displaySlot.transform.position}, item: {item.Name}");
            yield return new WaitUntil(() => !_agent.pathPending &&
                                                _agent.remainingDistance <= _agent.stoppingDistance);
            yield return new WaitForSeconds(3);

            if (IsInterestedInBuying(item))
            {
                Debug.Log($"Wants {item.Name}");
                _displaySlotController = displaySlotController;
                _itemToBuy = displaySlotController.GetItem();
                GoToCheckout();
                yield break;
            }
            else Debug.Log($"Doesnt want {item.Name}");
        }
        GoToExit();
    }

    public void GoToCheckout(){
        _agent.SetDestination(_counterPos.position);
    }

    private bool IsInterestedInBuying(ItemData item)
    {
        bool isInterested = false;
        int chanceToBuy = 0;
        if (_desiredItemsSO.GetDesiredItems().Exists(i=>i.ID==item.ID))
        {
            chanceToBuy = 90;
        }
        else chanceToBuy = 10;
        int random = UnityEngine.Random.Range(0, 100);
        Debug.Log($"Chance to buy: {chanceToBuy}, random: {random}");
        if (random < chanceToBuy)
        {
            isInterested = true;
        }
        else isInterested = false;
        return isInterested;
    }

    public ItemData GetItemToBuy(){
        return _itemToBuy;
    }
    
    public DisplaySlotController GetDisplaySlotController(){return _displaySlotController; }

    public int GetTolerance(){
        return _tolerance;
    }
    public void SetShopCheck(bool isInShop)
    {
        _isInShop = isInShop;
    }

    public void GoToExit(){
        _agent.speed = 3.5f;
        _agent.SetDestination(_despawnInShop.position);
    }

    public NPCType GetNpcType()
    {
        return _NPCType;
    }
}
