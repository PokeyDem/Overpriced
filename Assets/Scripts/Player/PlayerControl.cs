using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControl : SingletonDontDestroyOnLoad<PlayerControl>, PlayerInputs.IPlayerActions
{

    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _inertiaDecay = 2f;
    [SerializeField] private float _rotationSpeed = 10f;
    private Rigidbody _rb;
    private Vector3 _movementVector;
    private Vector3 _currentVelocity;
    private float _yPos;
    private IDataService _dataService = new JsonDataService();
    private PlayerInputs _playerInput;

    private IInteractable _currentInteractable;
    [SerializeField] private bool _disableControls;
    [SerializeField] private InteractUI _interactUI;


    private new void Awake()
    {
        base.Awake();
        PrepareInputSystem();
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _yPos = transform.position.y;
    }

    private void PrepareInputSystem()
    {
        _playerInput = new PlayerInputs();
        _playerInput.Player.SetCallbacks(this);
        _playerInput.Player.Enable();
    }

    void Update()
    {
        if (_disableControls && _playerInput.Player.enabled)
        {
            _playerInput.Player.Disable();
            _interactUI.SetActive(false);
        }
        else if (!_disableControls && !_playerInput.Player.enabled) {
            _playerInput.Player.Enable();
            if(_currentInteractable != null)
            {
                _interactUI.SetActive(true);
            }
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal"); 
        float verticalInput = Input.GetAxisRaw("Vertical");    
        
        //_movementVector = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        if (_movementVector != Vector3.zero)
        {
            _currentVelocity = _movementVector * _moveSpeed;
        }
        else
        {
            _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, Time.deltaTime * _inertiaDecay);
        }
        
        if (_currentVelocity != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_currentVelocity, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * _rotationSpeed);
        }
        

    }

    void FixedUpdate(){
        Vector3 newPos = _rb.position + _currentVelocity * Time.fixedDeltaTime;
        newPos.y = _yPos;
        _rb.MovePosition(newPos);
    }

    public bool IsMoving(){
        return _movementVector != Vector3.zero;
    }

    public PlayerData GetPlayerData(){
        return new PlayerData(transform.position.x, transform.position.y, transform.position.z);
    }

    public void LoadPlayer(PlayerData playerData){
        Vector3 savedPosition = new Vector3(playerData.x, playerData.y, playerData.z);
        transform.position = savedPosition;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            Vector2 vector= context.ReadValue<Vector2>();
            _movementVector = new Vector3(vector.x, 0f, vector.y);
        }
    }
    public void SetInteractable(IInteractable interactable) {
        _currentInteractable= interactable;
        if(interactable != null)
        {
            _interactUI.UpdateText(_currentInteractable.TriggerInteractPrompt());
            _interactUI.SetActive(true);
        } else { _interactUI.SetActive(false); }

    }
    public IInteractable GetInteractable() {  return _currentInteractable; }
    public void Interact()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
        else Debug.Log("Nothing to interact with");
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Interact();
        }
    }

    public void DisableControl(){
        _disableControls = true;
    }

    public void EnableControl(){
        _disableControls = false;
    }
}
