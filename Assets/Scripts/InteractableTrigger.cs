using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private TextMeshProUGUI _interactUIText;
    [SerializeField] private GameObject _interactableGameObject;
    [SerializeField] private IInteractable _interactable;

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
            _interactUIText.text = _interactable.TriggerInteractPrompt();
            _interactUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player") && _interactable != null)
        {
            _interactUI.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")&& Input.GetKey(KeyCode.E) && _interactable != null)
        {
            Debug.Log("Interact");
            _interactable.Interact();
        }
    }
    private void OnDisable()
    {
        if(_interactUI != null)
        {
            _interactUI.SetActive(false);
        }
    }
}
