//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Pool;

//public class NpcBehaviour : MonoBehaviour, IEmotable
//{

//    [SerializeField] private int _tolerance;
//    [SerializeField] private float _toleranceDecimal;
//    [SerializeField] private NPCType _NPCType;
//    public NavMeshAgent _agent{ get; private set; }
//    [SerializeField] private ItemData _itemToBuy; //Set by ChooseItem
//    public Transform _despawnPointPos { get; private set; }
//    public Transform _windowPos { get; private set; }
//    public Transform _doorPos { get; private set; }
//    public Transform _despawnInShop { get; private set; }
//    public DisplaySlotController _displaySlotController;
//    public List<Transform> _counterPos { get; private set; }
//    private bool _itemIsDesired;
//    private Vector3 _target;

//    [SerializeField] private bool _isInShop;
//    [SerializeField] private NPCDesiredItemsSO _desiredItemsSO;
//    private List<ItemData> _desiredItems;

//    public event Action Initialized;
//    public event Action DecidingStarted;
//    public event Action ItemRejected;
//    public event Action ItemSelected;
//    public event Action<MoodType> MoodChanged;

//    [SerializeField] private UpdateEmote _updateText;
//    private NPCEmotePresenter _presenter;
//    [SerializeField]private int _minChanceToBuy = 0;

//    [SerializeField] private List<DisplaySlotController> _displaySlotsWithItems;

//    private IObjectPool<NpcBehaviour> _pool;

//    private void Awake()
//    {
//        _presenter=new NPCEmotePresenter(this,_updateText);
//    }
//    private void Update()
//    {
//        if (_agent.hasPath)
//        {
//            FaceTarget(_agent.steeringTarget);
//        } else
//        {
//            FaceTarget(_target);
//        }
//    }

//    public void Initialize(bool isInShop,Vector3 spawnPoint, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, DisplaySlotController displaySlotController, List<Transform> counterPos, List<ItemData> desiredItems, IObjectPool<NpcBehaviour> pool){
//        _despawnPointPos = despawnPointPos;
//        _windowPos = windowPos;
//        _doorPos = doorPos;
//        _displaySlotController = displaySlotController;
//        _isInShop = isInShop;
//        _counterPos = counterPos;
//        _despawnInShop = despawnInShop;
//        _desiredItems = desiredItems;
//        _pool = pool;

//        _agent = GetComponent<NavMeshAgent>();
//        float random = UnityEngine.Random.Range(100, 250);
//        _agent.speed = random / 100;
//        _agent.Warp(spawnPoint);
//        Initialized?.Invoke();

//        /*
//        if (!_isInShop)
//        {
//            StartCoroutine(CheckItemsThroughWindow());
//        }
//        else
//        {
//            //Debug.Log("Spawned in shop");
//            StartCoroutine(BrowseDisplays());
//        }
//        */
//    }



//    private IEnumerator CheckItemsThroughWindow()
//    {
//        float randomX=UnityEngine.Random.Range(-100,100);
//        randomX = randomX / 100;
//        float randomZ= UnityEngine.Random.Range(-10, 60);
//        randomZ = randomZ / 100;
//        Vector3 deviation=new Vector3(randomX,0,randomZ);
//        _agent.SetDestination(_windowPos.position+deviation);
//        yield return new WaitUntil(() => !_agent.pathPending &&
//                                    _agent.remainingDistance <= _agent.stoppingDistance);
//        //StartCoroutine(LerpRotation( 90));
//        _target = _windowPos.position + deviation + new Vector3(0, 0, -1);
//        yield return new WaitForSeconds(WaitRandomAmount(200, 600));
//        if (DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count != 0)
//        {
//            _agent.SetDestination(_doorPos.position);
//        }
//        else
//        {
//            _agent.SetDestination(_despawnPointPos.position);
//            //Debug.Log("Item wasn't found, Npc destination: DespawnPoint");
//        }
//    }

//    public void StartBrowsing()
//    {
//        StartCoroutine(BrowseDisplays());
//    }
//    private IEnumerator BrowseDisplays()
//    {
//        //Debug.Log($"BrowseDisplays started");
//        _displaySlotsWithItems = new List<DisplaySlotController>(DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems());
//        //Queue<DisplaySlotController> displaySlotsWithItems = new Queue<DisplaySlotController>(DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems());
//        //foreach (var displaySlot in displaySlotsWithItems)
//        //for (int i=0;i<displaySlotsWithItems.Count;i++)
//        while (_displaySlotsWithItems.Count > 0)
//        {
//            _itemIsDesired = false;
//            //int nrOfPreferredItemsOnDisplay = _displaySlotsWithItems.FindAll(ds => ds.GetItem() != null&&!ds.isChosen && _desiredItemsSO.GetDesiredItems().Exists(i => i.ID == ds.GetItem().ID)).Count;
//            DisplaySlotController displaySlotController = _displaySlotsWithItems[0];
//            _displaySlotsWithItems.RemoveAt(0);
//            ItemData item = displaySlotController.GetItem();
//            if (item==null|| displaySlotController == null)
//            {
//                continue;
//            }
//            if (displaySlotController.isChosen)
//            {
//                //Debug.Log($"Chosen");
//                continue;
//            }
//            if (!displaySlotController.isOccupied)
//            {
//                displaySlotController.isOccupied = true;
//            } 
//            else
//            {
//                //Debug.Log($"Occupied");
//                if (_displaySlotsWithItems.Find(ds => !ds.isOccupied) != null)
//                {
//                    _displaySlotsWithItems.Add(displaySlotController);
//                    yield return new WaitForSeconds(0.1f);
//                    continue;
//                }
//                else
//                {
//                    yield return new WaitUntil(() => !displaySlotController.isOccupied || displaySlotController.isChosen);
//                }
//            }

//            if (displaySlotController.isChosen)
//            {
//                //Debug.Log($"Chosen");
//                continue;
//            }
//            //_agent.speed = 2;
//            _agent.SetDestination(displaySlotController.transform.position);
//            _target=displaySlotController.transform.position;
//            //Debug.Log($"Going to: {displaySlotController.transform.position}, item: {item.Name}");
//            yield return new WaitUntil(() => !_agent.pathPending &&
//                                                _agent.remainingDistance <= _agent.stoppingDistance);
//            //StartCoroutine(LerpRotation(Vector3.Angle(transform.position, displaySlotController.transform.position)));
//            transform.LookAt(displaySlotController.transform.position);
//            yield return new WaitForSeconds(0.2f);
//            DecidingStarted?.Invoke();
//            _itemIsDesired = _desiredItems.Exists(i => i.ID == item.ID);
//            if (!_itemIsDesired)
//            {
//                yield return new WaitForSeconds(2);
//            } else 
//                yield return new WaitForSeconds(WaitRandomAmount(500, 1000));
//            //_minChanceToBuy = 100 - (10 * nrOfPreferredItemsOnDisplay);
//            if(!displaySlotController.isChosen)
//            {
//                if (IsInterestedInBuying(item))
//                {
//                    //Debug.Log($"Wants {item.Name}");
//                    _displaySlotController = displaySlotController;
//                    _itemToBuy = displaySlotController.GetItem();
//                    displaySlotController.isOccupied = true;
//                    displaySlotController.isChosen = true;
//                    ItemSelected?.Invoke();
//                    GoToCheckout();
//                    yield break;
//                }
//                else {
//                    displaySlotController.isOccupied = false;
//                    ItemRejected?.Invoke();
//                    //Debug.Log($"Doesnt want {item.Name}"); 
//                }
//            }
//        }
//        GoToExitWithoutItem();
//    }

//    public void GoToCheckout(){
//        //_agent.SetDestination(_counterPos.position);
//        StartCoroutine(StandInLine());
//    }

//    private IEnumerator StandInLine()
//    {
//        int currentPosInLine=-1;
//        _agent.SetDestination(_counterPos[0].position);
//        _target = _counterPos[0].position;
//        yield return new WaitForSeconds(0.1f);
//        yield return new WaitUntil(() => _agent.remainingDistance < 2.5f);
//        _agent.speed = 1.5f;
//        Transform destination = this.transform;
//        while (destination != _counterPos[0])
//        {
//            for (int i = 0; i < _counterPos.Count; i++)
//            {
//                if (NpcManager.counterTaken[i])
//                {
//                    continue;
//                }
//                else
//                {
//                    if(currentPosInLine>=0)
//                    {
//                        NpcManager.counterTaken[currentPosInLine] = false;
//                    }
//                    NpcManager.counterTaken[i] = true;
//                    currentPosInLine = i;
//                    destination = _counterPos[i];
//                    break;
//                }
//            }
//            _agent.SetDestination(destination.position);
            
//            //Debug.Log($"Going to: {destination.position}, Line");
//            if (currentPosInLine == 0) {
//                yield return new WaitUntil(() => !_agent.pathPending &&
//                                                 _agent.remainingDistance <= _agent.stoppingDistance);
//                //StartCoroutine(LerpRotation(Vector3.Angle(transform.position, destination.position)));
//            }else if (currentPosInLine>0) {
//                yield return new WaitUntil(() => !NpcManager.counterTaken[currentPosInLine - 1]);//if in line wait till next in line is open
//            }else {
//                yield return new WaitUntil(() => !NpcManager.counterTaken[_counterPos.Count-1]);//if not in line wait till last in line is open
//            }
//        }
//        _target = _counterPos[0].position + new Vector3(0, 0, -1);
//    }
//    private float WaitRandomAmount(int min,int max)//min and max in milliseconds
//    {
//        float random = UnityEngine.Random.Range(min, max);
//        random = random / 100;
//        return random;
//    }

//    public bool IsInterestedInBuying(ItemData item)
//    {
//        bool isInterested = false;
//        int chanceToBuy = 0;
//        if (_itemIsDesired)
//        {
//            chanceToBuy = 90;//Math.Max(_minChanceToBuy,60);
//        }
//        else chanceToBuy = 10;
//        int random = UnityEngine.Random.Range(0, 100);
//        //Debug.Log($"Chance to buy: {chanceToBuy}, random: {random}");
//        if (random < chanceToBuy)
//        {
//            isInterested = true;
//        }
//        else isInterested = false;
//        return isInterested;
//    }

//    public ItemData GetItemToBuy(){
//        return _itemToBuy;
//    }
    
//    public DisplaySlotController GetDisplaySlotController(){return _displaySlotController; }

//    public int GetTolerance(){
//        return _tolerance;
//    }
//    public float GetToleranceDecimal()
//    {
//        return _toleranceDecimal;
//    }

//    public void GoToExit(bool itemSold)
//    {
//        if (!itemSold)
//        {
//            ItemRejected?.Invoke();
//        }
//        _displaySlotController.isChosen = false;
//        _displaySlotController.isOccupied = false;
//        NpcManager.counterTaken[0] = false;
//        _agent.speed = 2.5f;
//        _agent.SetDestination(_despawnInShop.position);
//    }

//    public void GoToExitWithoutItem()
//    {
//        _agent.speed = 2.5f;
//        _agent.SetDestination(_despawnInShop.position);
//    }

//        public NPCType GetNpcType()
//    {
//        return _NPCType;
//    }

//    public void ReturnToPool()
//    {
//        if (_pool != null)
//        {
//            _pool.Release(this);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//    void FaceTarget(Vector3 target)
//    {
//        Vector3 direction = (target - transform.position).normalized;
//        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);
//    }
//    public void WarpNPC(Vector3 spawnPoint)
//    {
//        _agent.Warp(spawnPoint);
//    }
//    public void Debg(string x)
//    {
//        Debug.Log(x);
//    }
//}
