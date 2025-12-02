using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuButtonsRaycastHandler : MonoBehaviour
{
   [SerializeField] private Camera camera;
   [SerializeField] private Transform _cashRegister;

   [SerializeField] private string _ButtonsLayerMaskName;
   [SerializeField] private Material _HoverMaterial;
   private IHoverableObject _lastHoverableObject;
   private GameObject _lastHoverableGameObject;
   private bool _isHovering;

   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         ButtonRayCast();
      }
      HoverRayCast();
   }

   private void ButtonRayCast()
   {
      RaycastHit hit;
      Ray ray = camera.ScreenPointToRay(Input.mousePosition);
      Debug.DrawRay(ray.origin, ray.direction, Color.red, 1000);
         
      if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("InteractableButtons"))) {
         hit.transform.gameObject.GetComponent<IInteractableMenuButton>().Interact();
      }
   }

   private void HoverRayCast()
   {
      RaycastHit hit;
      Ray ray = camera.ScreenPointToRay(Input.mousePosition);
         
      if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("InteractableButtons")) 
          && hit.transform.gameObject.CompareTag("HoverableObject") && MainMenuManager.Instance.IsInSubMenu())
      {
         if (_lastHoverableGameObject != hit.transform.gameObject)
         {
            _lastHoverableObject = hit.transform.gameObject.GetComponent<IHoverableObject>();
            _lastHoverableGameObject =  hit.transform.gameObject;
         }

         if (!_isHovering)
         {
            _lastHoverableObject.OnHoverEnter();
            _isHovering = true;
         }
      }
      else
      {
         if (_isHovering)
         {
            _lastHoverableObject.OnHoverExit();
            _isHovering = false;
         }
      }
   }
}
