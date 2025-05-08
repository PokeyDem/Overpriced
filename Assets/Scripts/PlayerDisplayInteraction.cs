using UnityEngine;

public class PlayerDisplayInteraction : MonoBehaviour, IInteractable
{

    [SerializeField] private float _interactionRange;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerControl _playerControl;
    private LayerMask _slotsLayer;
    private DisplaySlotController _nearestDisplaySlotController;
    private GameObject _nearestSlot;
    private GameObject _lastNearestSlot;

    private void Awake(){
        _slotsLayer = LayerMask.GetMask("Slots");
        //_inputManager.OnInteraction += EnableInventory;
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
       
        if (_lastNearestSlot) //changed order with if(_nearestSlot), somehow works better -K
        {
            distance = Mathf.Abs(Vector3.Distance(transform.position, _lastNearestSlot.transform.position));
            if (distance > _interactionRange)
            {
                _lastNearestSlot.GetComponent<DisplaySlotController>().DisableMarker();
            }

            if (!_nearestSlot)
            {
                _inventoryManager.DisableInventory();
                if (_playerControl.GetInteractable() == this as IInteractable)
                {
                    _playerControl.SetInteractable(null);
                }
            }
        }

        if (_nearestSlot){
            if (_lastNearestSlot) 
                _lastNearestSlot.GetComponent<DisplaySlotController>().DisableMarker();
            _lastNearestSlot = _nearestSlot;
            _nearestDisplaySlotController = _nearestSlot.GetComponent<DisplaySlotController>();
            _nearestDisplaySlotController.EnableMarker();
            if (_playerControl.GetInteractable() != this as IInteractable && _playerControl.GetInteractable() != _inventoryManager as IInteractable)
            {
                _playerControl.SetInteractable(this);
            }
        }
    }
    
    private void EnableInventory(){
        if (!_nearestSlot) return;
        _inventoryManager.EnableInventory();
    }

    public void Interact()
    {
        EnableInventory();
    }

    public string TriggerInteractPrompt()
    {
        return "Open Inventory";
    }
}
