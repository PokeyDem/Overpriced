using UnityEngine;

public class PlayerDisplayInteraction : MonoBehaviour{

    [SerializeField] private float _interactionRange;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private InputManager _inputManager;
    private LayerMask _slotsLayer;
    private DisplaySlotController _nearestDisplaySlotController;
    private GameObject _nearestSlot;
    private GameObject _lastNearestSlot;

    private void Awake(){
        _slotsLayer = LayerMask.GetMask("Slots");
        _inputManager.OnInteraction += EnableInventory;
    }

    private void Update(){
        DetectNearestSlot();
        _inventoryManager.SetNearestSlot(_nearestDisplaySlotController);
    }

    private void DetectNearestSlot(){
        Collider[] slots = Physics.OverlapSphere(transform.position, _interactionRange, _slotsLayer);
        float distance = -1;
        _nearestSlot = null;
        
        float closestDistance = float.MaxValue;
        foreach (var slot in slots){
            distance = Mathf.Abs(Vector3.Distance(transform.position, slot.transform.position));
            if (distance < closestDistance ){
                closestDistance = distance;
                _nearestSlot = slot.gameObject; // not sure if it is getting right gameobject
            }
        }
        
        if (_nearestSlot){
            if (_lastNearestSlot) 
                _lastNearestSlot.GetComponent<DisplaySlotController>().DisableMarker();
            _lastNearestSlot = _nearestSlot;
            _nearestDisplaySlotController = _nearestSlot.GetComponent<DisplaySlotController>();
          
            _nearestDisplaySlotController.EnableMarker();
        }

        if (_lastNearestSlot){ 
            distance = Mathf.Abs(Vector3.Distance(transform.position, _lastNearestSlot.transform.position));
            if (distance > _interactionRange)
                _lastNearestSlot.GetComponent<DisplaySlotController>().DisableMarker();
            if (!_nearestSlot)
                _inventoryManager.DisableInventory();
        }
    }
    
    private void EnableInventory(){
        if (!_nearestSlot) return;
        _inventoryManager.EnableInventory();
    }
}
