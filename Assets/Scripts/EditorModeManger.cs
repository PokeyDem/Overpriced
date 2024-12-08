using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorModeManger : MonoBehaviour{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _prefabPreview;
    [SerializeField] private float _gridSize = 1f;
    [SerializeField] private float _gridDeadZone = 1f;
    private bool _isEditing;
    private LayerMask _floorLayer;
    
    private void Awake(){
        _floorLayer = LayerMask.GetMask("Floor");
        _prefabPreview = Instantiate(_prefabPreview);
        _prefabPreview.SetActive(false);
    }

    private void Update(){
        if (_isEditing) 
            UpdatePreviewPos();
        
        if (Input.GetMouseButtonDown(0) && _isEditing){
            PlacePrefab();
        }

        if (Input.GetKeyDown(KeyCode.F)){
            _isEditing = !_isEditing;
            _prefabPreview.SetActive(_isEditing);
        }
    }
    private void PlacePrefab(){
        if (_prefabPreview.activeSelf){
            Instantiate(_prefab, _prefabPreview.transform.position, _prefabPreview.transform.rotation);
        }
    }

    private void UpdatePreviewPos(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _floorLayer)){
            _prefabPreview.SetActive(true);
            
            Vector3 hitPoint = hit.point;
            Vector3 snappedPos = SnapToGrid(hitPoint);
            
            _prefabPreview.transform.position = snappedPos;
            _prefabPreview.transform.rotation = Quaternion.identity;
        }
        else{
            _prefabPreview.SetActive(false);
        }
    }

    private Vector3 SnapToGrid(Vector3 pos){
        float x = Mathf.Round(pos.x / _gridSize) * _gridSize;
        float z = Mathf.Round(pos.z / _gridSize) * _gridSize;
        float y = pos.y + _prefab.transform.localScale.y / 2;
        Debug.Log("X: " + x + " Y: " + pos.y  + " Z: " + z);
        return new Vector3(x, y, z);
    }
}
