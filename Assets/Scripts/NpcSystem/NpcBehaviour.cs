using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcBehaviour : MonoBehaviour{

    [SerializeField] private int _tolerance;
    [SerializeField] private NPCType _NPCType;
    private NavMeshAgent _agent;
    [SerializeField] private ItemData _itemToBuy; //Set by ChooseItem
    private Transform _despawnPointPos;
    private Transform _windowPos; 
    private Transform _doorPos;
    private Transform _despawnInShop;
    private GameObject _displayItemSlot;
    private Transform _counterPos;

    [SerializeField] private bool _isInShop;
    private IItemSelector _itemSelector;
    private List<ItemData> _desiredItems;
    private Dictionary<GameObject, ItemData> _desiredItemsOnDisplays = new Dictionary<GameObject, ItemData>();//keys are displaySlots, values items on the display
    private List<GameObject> _occupiedDisplays = new List<GameObject>();
    [SerializeField] private ItemsDatabaseSO _itemDatabaseSO;

    private void Awake()
    {
        _itemSelector = GetComponent<IItemSelector>();
        if (_itemSelector != null)
        {
            _desiredItems = _itemSelector.SelectDesiredItems();
        }
    }

    public void Initialize(ItemData item, bool isInShop, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, GameObject displayItemSlot, Transform counterPos, Dictionary<GameObject, ItemData> desiredItemsOnDisplays, List<GameObject> occupiedDisplays){
        if (item == null)
        {
        }
        else _itemToBuy = item;
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos;
        _doorPos = doorPos;
        _displayItemSlot = displayItemSlot;
        _isInShop = isInShop;
        _counterPos = counterPos;
        _despawnInShop = despawnInShop;
        _desiredItemsOnDisplays = desiredItemsOnDisplays;
        _occupiedDisplays = occupiedDisplays;
    }

    private void Start(){
        _agent = GetComponent<NavMeshAgent>();
        if (!_isInShop){
            _agent.SetDestination(_despawnPointPos.position);
            Debug.Log("Npc destination: DespawnPoint");
        }
        else{
            Debug.Log("Spawned in shop");
            StartCoroutine(BrowseDisplays());
        }
    }


    public void FindDisplaysWithItems()
    {
        _agent.SetDestination(_windowPos.position);
        _occupiedDisplays = new List<GameObject>();
        foreach (GameObject displayItemSlot in GameObject.FindGameObjectsWithTag("DisplayItemSlot"))
        {
            DisplaySlotController slotController = displayItemSlot.GetComponent<DisplaySlotController>();
            if(slotController.GetItemId()!=-1)
            {
                _occupiedDisplays.Add(displayItemSlot);
            }
        }
            StartCoroutine(LookingDelay());
    }


    private IEnumerator LookingDelay()
    {
        yield return new WaitForSeconds(2);
        if (_occupiedDisplays.Count != 0)
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
        Debug.Log($"BrowseDisplays started, Count of items: {_occupiedDisplays.Count}");
        foreach (var displaySlot in _occupiedDisplays)
        {
            DisplaySlotController displaySlotController = displaySlot.GetComponent<DisplaySlotController>();
            ItemData item = _itemDatabaseSO._itemsData.Find(item => item.ID == displaySlotController.GetItemId());

            _agent.speed = 2;
            _agent.SetDestination(displaySlot.transform.position);
            Debug.Log($"Going to: {displaySlot.transform.position}, item: {item.Name}");
            yield return new WaitUntil(() => !_agent.pathPending &&
                                                _agent.remainingDistance <= _agent.stoppingDistance);
            yield return new WaitForSeconds(3);

            if (IsInterestedInBuying(item))
            {
                Debug.Log($"Wants {item.Name}");
                _itemToBuy = item;
                _displayItemSlot = displaySlot;
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
        if (_desiredItems.Contains(item))
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
    
    public GameObject GetDisplayItemSlot(){return _displayItemSlot; }

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
    public Dictionary<GameObject, ItemData> GetDesiredItemsOnDisplays()
    {
        return _desiredItemsOnDisplays;
    }
    public List<GameObject> GetOccupiedDisplays()
    {
        return _occupiedDisplays;
    }
}
