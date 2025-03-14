using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
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
    private bool _isInShop;
    private bool _lookingForItem;
    private IItemSelector _itemSelector;
    private List<ItemData> _desiredItems;

    private void Awake()
    {
        _itemSelector = GetComponent<IItemSelector>();
        if (_itemSelector != null)
        {
            _desiredItems = _itemSelector.SelectDesiredItems();
        }
        _lookingForItem = true;
    }

    public void Initialize(ItemData item, bool isInShop, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, GameObject displayItemSlot, Transform counterPos){
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
    }

    public NPCType GetNpcType()
    { 
        return _NPCType; 
    }
    private void Start(){
        _agent = GetComponent<NavMeshAgent>();
        if (!_isInShop){
            _agent.SetDestination(_despawnPointPos.position);
            Debug.Log("Npc destination: DespawnPoint");
        }
        else{
            StartCoroutine(GoToCheckout()); //Changed For tests
        }
    }

    public void CheckItemsOnDisplays(){
        if (_desiredItems != null)
        {
            _agent.SetDestination(_windowPos.position);
            string itemNames = String.Join(", ", _desiredItems.Select(p => p.Name));//delete Linq library if delete this
            Debug.Log($"{_NPCType} looking for: {itemNames}");
            foreach (ItemData i in _desiredItems) 
            {
                foreach (GameObject displayItemSlot in GameObject.FindGameObjectsWithTag("DisplayItemSlot"))
                {
                    DisplaySlotController slotController = displayItemSlot.GetComponent<DisplaySlotController>();
                    if (slotController.GetItemId() == i.ID)
                    {
                            Debug.Log($"Item matched, ID: {i.ID}");
                            _displayItemSlot = displayItemSlot;
                            _itemToBuy = _desiredItems.Find(x=>x.ID==slotController.GetItemId());//first desired item found
                            break;
                    }
                }
                if (_displayItemSlot)
                    break;
            }
        }
        Debug.Log(_itemToBuy.Name);
        StartCoroutine(LookingDelay());
    }
    public void CheckItemOnDisplay(GameObject displayItemSlot)
    {
        if (_desiredItems != null)
        {
            ItemData itemOnDisplay=_desiredItems.Find(item => item.ID== displayItemSlot.GetComponent<DisplaySlotController>().GetItemId());
            if(itemOnDisplay != null)
            {
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                float buyChance = 90;
                if (randomValue < buyChance)
                {
                    _displayItemSlot = displayItemSlot;
                    _itemToBuy = itemOnDisplay;
                    _lookingForItem = false;
                }
            }
        }
    }

    private IEnumerator LookingDelay(){
        yield return new WaitForSeconds(2);
        if (_displayItemSlot)
        {
            _agent.SetDestination(_doorPos.position);
        }
        else{
            _agent.SetDestination(_despawnPointPos.position);
            Debug.Log("Item wasn't found, Npc destination: DespawnPoint");
        }
    }

    public void GoToDisplay(){
        _agent.SetDestination(new Vector3(_displayItemSlot.transform.position.x + 1.5f, _displayItemSlot.transform.position.y, _displayItemSlot.transform.position.z));
        _displayItemSlot.GetComponentInChildren<DisplayTrigger>().SetIsEnabled(true);
        Debug.Log("Npc destination: Display");
    }

    public IEnumerator GoToCheckout(){
        //yield return new WaitForSeconds(0);
        //_display.GetComponentInChildren<DisplayTrigger>().SetIsEnabled(false);
        _agent.SetDestination(_counterPos.position);
        _agent.speed = 1;
        yield return new WaitForSeconds(1);
        _agent.isStopped=true;
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
        _agent.SetDestination(_despawnInShop.position);
    }
    
}
