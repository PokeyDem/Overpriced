using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorModeManger : MonoBehaviour{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _prefabPreview;
    [SerializeField] private float _gridSize = 1f;
    [SerializeField] private float _gridDeadZone = 1f;
    [SerializeField] private Vector3 _min;
    [SerializeField] private Vector3 _max;
    private Material _previewMat;
    private Color _previewColor = new Color(5, 5, 5, 50);
    private Color _outOfBoundsColor = new Color(218, 0, 0, 50);
    
    private bool _isInBound = true;
    private bool _isEditing;
    private LayerMask _floorLayer;
    
    private void Awake(){
        _floorLayer = LayerMask.GetMask("Floor");
        _prefabPreview = Instantiate(_prefabPreview);
        _prefabPreview.SetActive(false);
        _previewMat = _prefabPreview.GetComponent<Renderer>().material;
        _previewColor = _previewMat.color;
        _outOfBoundsColor = _previewColor;
        _outOfBoundsColor.r = 170;
        _outOfBoundsColor.g = 0;
        _outOfBoundsColor.b = 0;
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
        if (_prefabPreview.activeSelf && _isInBound){
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
            
            IsInBounds(snappedPos);

            if (_isInBound){
                _previewMat.color = _previewColor;
            }
            else{
                _previewMat.color = _outOfBoundsColor;
            }
            
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

    private void IsInBounds(Vector3 pos){
        _isInBound = pos.x <= _max.x && pos.x >= _min.x && pos.z <= _max.z && pos.z >= _min.z && pos.y < _max.y;
        Debug.Log("Pos y: " + pos.y + " max y: " + _max.y);
    }
}
