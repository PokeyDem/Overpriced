using UnityEngine;

public class PlayerDisplayInteraction : MonoBehaviour{

    [SerializeField] private float _interactionRange;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private InputManager _inputManager;
    private LayerMask _slotsLayer;
    private SlotController _nearestSlotController;
    private GameObject _nearestSlot;
    private GameObject _lastNearestSlot;

    private void Awake(){
        _slotsLayer = LayerMask.GetMask("Slots");
    }

    private void Update(){
        DetectNearestSlot();
        _inputManager.OnInteraction += EnableInventory;
    }

    private void DetectNearestSlot(){
        Collider[] slots = Physics.OverlapSphere(transform.position, _interactionRange, _slotsLayer);
        float distance;
        _nearestSlot = null;
        
        float closestDistance = float.MaxValue;
        foreach (var slot in slots){
            distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < closestDistance && distance < _interactionRange){
                closestDistance = distance;
                _nearestSlot = slot.gameObject; // not sure if it is getting right gameobject
            }
            
        }
        
        if (_nearestSlot){
            if (_lastNearestSlot) 
                _lastNearestSlot.GetComponent<SlotController>().DisableMarker();
            _lastNearestSlot = _nearestSlot;
            _nearestSlotController = _nearestSlot.GetComponent<SlotController>();
            _nearestSlotController.EnableMarker();
        }

        if (_lastNearestSlot){ 
            distance = Vector3.Distance(transform.position, _lastNearestSlot.transform.position);
            if (distance > _interactionRange)
                _lastNearestSlot.GetComponent<SlotController>().DisableMarker();
        }

    }
    

    private void EnableInventory(){
        if (!_nearestSlot) return;
        _inventoryManager.EnableInventory(_nearestSlotController);
    }
}
