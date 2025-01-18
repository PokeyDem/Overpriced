using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcBehaviour : MonoBehaviour{

    [SerializeField] private int _tolerance;
    private NavMeshAgent _agent;
    private ItemData _itemToBuy; //Set by NpcManager
    private Transform _despawnPointPos;
    private Transform _windowPos; 
    private Transform _doorPos;
    private Transform _despawnInShop;
    private GameObject _display;
    private Transform _counterPos;
    private bool _isInShop;

    public void Initialize(ItemData item, bool isInShop, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, GameObject display, Transform counterPos){
        _itemToBuy = item;
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos;
        _doorPos = doorPos;
        _display = display;
        _isInShop = isInShop;
        _counterPos = counterPos;
        _despawnInShop = despawnInShop;
    }
    private void Start(){
        _agent = GetComponent<NavMeshAgent>();
        if (!_isInShop){
            _agent.SetDestination(_despawnPointPos.position);
            Debug.Log("Npc destination: DespawnPoint");
        }
        else{
            GoToDisplay();
        }
    }

    private void Update(){
        // if (_agent.remainingDistance <= _agent.stoppingDistance){
        //     _agent.ResetPath();
        // }
    }

    public void CheckItemsOnDisplays(){
        _agent.SetDestination(_windowPos.position);
        foreach (GameObject display in GameObject.FindGameObjectsWithTag("Display")){
            foreach (var slotController in display.GetComponentsInChildren<DisplaySlotController>()){
                if (slotController.GetItemId() == _itemToBuy.ID){
                    Debug.Log("Item matched");
                    _display = display;
                    break;
                }
            }
            if (_display)
                break;
        }

        StartCoroutine(LookingDelay());
    }

    private IEnumerator LookingDelay(){
        yield return new WaitForSeconds(2);
        if (_display){
            _agent.SetDestination(_doorPos.position);
        }
        else{
            _agent.SetDestination(_despawnPointPos.position);
            Debug.Log("Item wasn't found, Npc destination: DespawnPoint");
        }
    }

    public void GoToDisplay(){
        _agent.SetDestination(new Vector3(_display.transform.position.x + 1.5f, _display.transform.position.y, _display.transform.position.z));
        _display.GetComponentInChildren<DisplayTrigger>().SetIsEnabled(true);
        Debug.Log("Npc destination: Display");
    }

    public IEnumerator GoToCheckout(){
        yield return new WaitForSeconds(2);
        _display.GetComponentInChildren<DisplayTrigger>().SetIsEnabled(false);
        _agent.SetDestination(_counterPos.position);
    }

    public ItemData GetItemToBuy(){
        return _itemToBuy;
    }
    
    public GameObject GetDisplay(){return _display;}

    public int GetTolerance(){
        return _tolerance;
    }

    public void GoToExit(){
        _agent.SetDestination(_despawnInShop.position);
    }
    
}
