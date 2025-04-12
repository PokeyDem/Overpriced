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
    private List<Transform> _counterPos;

    [SerializeField] private bool _isInShop;
    [SerializeField] private NPCDesiredItemsSO _desiredItemsSO;

    public event Action BrowsingStarted;
    public event Action DecidingStarted;
    public event Action ItemRejected;
    public event Action ItemSelected;
    [SerializeField] private UpdateText _updateText;
    private NPCEmotePresenter _presenter;
    [SerializeField]private int _minChanceToBuy = 0;

    private void Awake()
    {
        _presenter=new NPCEmotePresenter(this,_updateText);
    }

    public void Initialize(bool isInShop, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, DisplaySlotController displaySlotController, List<Transform> counterPos){
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
        float random = UnityEngine.Random.Range(100,300);
        _agent.speed = random / 100;
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
        BrowsingStarted?.Invoke();
        List < DisplaySlotController > displaySlotsWithItems = new List<DisplaySlotController>(DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems());
        //Queue<DisplaySlotController> displaySlotsWithItems = new Queue<DisplaySlotController>(DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems());
        //foreach (var displaySlot in displaySlotsWithItems)
        //for (int i=0;i<displaySlotsWithItems.Count;i++)
        while (displaySlotsWithItems.Count > 0)
        {
            int nrOfPreferredItemsOnDisplay = displaySlotsWithItems.FindAll(ds =>!ds.isChosen && _desiredItemsSO.GetDesiredItems().Contains(ds.GetItem())).Count;
            DisplaySlotController displaySlotController = displaySlotsWithItems[0];
            displaySlotsWithItems.RemoveAt(0);
            ItemData item = displaySlotController.GetItem();
            if (item==null|| displaySlotController == null)
            {
                continue;
            }
            if (displaySlotController.isChosen)
            {
                Debug.Log($"Chosen");
                continue;
            }
            if (!displaySlotController.isOccupied)
            {
                displaySlotController.isOccupied = true;
            } 
            else
            {
                Debug.Log($"Occupied");
                if (displaySlotsWithItems.Find(ds => ds.isOccupied) != null)
                {
                    displaySlotsWithItems.Add(displaySlotController);
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                else
                {
                    yield return new WaitUntil(() => !displaySlotController.isOccupied || displaySlotController.isChosen);
                }
            }

            if (displaySlotController.isChosen)
            {
                Debug.Log($"Chosen");
                continue;
            }
            //_agent.speed = 2;
            _agent.SetDestination(displaySlotController.transform.position);
            Debug.Log($"Going to: {displaySlotController.transform.position}, item: {item.Name}");
            yield return new WaitUntil(() => !_agent.pathPending &&
                                                _agent.remainingDistance <= _agent.stoppingDistance);
            yield return new WaitForSeconds(0.2f);
            DecidingStarted?.Invoke();
            float random = UnityEngine.Random.Range(200, 1500);
            random = random / 100;
            yield return new WaitForSeconds(random);
            _minChanceToBuy = 100 - (10 * nrOfPreferredItemsOnDisplay);
            if (IsInterestedInBuying(item))
            {
                Debug.Log($"Wants {item.Name}");
                _displaySlotController = displaySlotController;
                _itemToBuy = displaySlotController.GetItem();
                displaySlotController.isOccupied = true;
                displaySlotController.isChosen = true;
                ItemSelected?.Invoke();
                GoToCheckout();
                yield break;
            }
            else {
                displaySlotController.isOccupied = true;
                ItemRejected?.Invoke();
                Debug.Log($"Doesnt want {item.Name}"); 
            }
        }
        GoToExitWithoutItem();
    }

    public void GoToCheckout(){
        //_agent.SetDestination(_counterPos.position);
        StartCoroutine(StandInLine());
    }

    private IEnumerator StandInLine()
    {
        Transform destination = this.transform;
        int currentPosInLine=-1;
        while (destination != _counterPos[0])
        {
            for (int i = 0; i < _counterPos.Count; i++)
            {
                if (NpcManager.counterTaken[i])
                {
                    continue;
                }
                else
                {
                    if(currentPosInLine>=0)
                    {
                        NpcManager.counterTaken[currentPosInLine] = false;
                    }
                    NpcManager.counterTaken[i] = true;
                    currentPosInLine = i;
                    destination = _counterPos[i];
                    break;
                }
            }
            _agent.SetDestination(destination.position);
            Debug.Log($"Going to: {destination.position}, Line");
            if (currentPosInLine>0)
            {
                yield return new WaitUntil(() => !NpcManager.counterTaken[currentPosInLine - 1]);//if in line wait till next in line is open
            }
            else
            {
                yield return new WaitUntil(() => !NpcManager.counterTaken[_counterPos.Count-1]);//if not in line wait till last in line is open
            }
        }
    }

    public bool IsInterestedInBuying(ItemData item)
    {
        bool isInterested = false;
        int chanceToBuy = 0;
        if (_desiredItemsSO.GetDesiredItems().Exists(i=>i.ID==item.ID))
        {
            chanceToBuy = _minChanceToBuy;
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

    public void GoToExit()
    {
        _displaySlotController.isChosen = false;
        NpcManager.counterTaken[0] = false;
        _agent.speed = 3.5f;
        _agent.SetDestination(_despawnInShop.position);
    }

    public void GoToExitWithoutItem()
    {
        _agent.speed = 3.5f;
        _agent.SetDestination(_despawnInShop.position);
    }

        public NPCType GetNpcType()
    {
        return _NPCType;
    }
}
