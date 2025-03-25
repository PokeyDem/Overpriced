using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour{
    [SerializeField] Camera _sceneCamera;
    [SerializeField] LayerMask _placementLayerMask;
    [SerializeField] private bool _isPlacementEnabled;
    private Vector3 _lastPosition;

    public event Action OnClicked, OnSwitch, OnInteraction;

    private void Awake(){
        _placementLayerMask = LayerMask.GetMask("Grid");
    }

    private void Update(){
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        
        if (Input.GetKeyDown(KeyCode.F) && _isPlacementEnabled)
            OnSwitch?.Invoke();
        
        if (Input.GetKeyDown(KeyCode.E))
            OnInteraction?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject(); 
    
    public Vector3 GetSelectedMapPosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _sceneCamera.nearClipPlane;
        Ray ray = _sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _placementLayerMask)){
            _lastPosition = hit.point;
        }
        return _lastPosition;
    }
}
