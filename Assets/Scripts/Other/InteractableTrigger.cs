using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _interactableGameObject;
    [SerializeField] private IInteractable _interactable;
    [SerializeField] private PlayerControl _playerControl;

    private void Awake()
    {
        if(_interactableGameObject!=null)
        {
            _interactableGameObject.TryGetComponent(out _interactable);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&_interactable!=null)
        {
            _playerControl.SetInteractable(_interactable);

        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player") && _interactable != null)
        {
            if(_playerControl.GetInteractable()==_interactable)
            {
                _playerControl.SetInteractable(null);
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _interactable != null && _playerControl.GetInteractable()!=_interactable)
        {
            Debug.Log("Interact");
            _playerControl.SetInteractable(_interactable);
        }
    }
    private void OnDisable()
    {

        if (_playerControl.GetInteractable() == _interactable)
        {
            _playerControl.SetInteractable(null);
        }
    }
}
