using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlacementSystem : MonoBehaviour{
    [SerializeField] InputManager _inputManager;
    [SerializeField] Grid _grid;
    [SerializeField] ObjectsDatabaseSO _objectsDatabase;
    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private Canvas _uiPanel;
    [SerializeField] PreviewSystem _previewSystem;
    private int _selectedObjectIndex = -1;
    private bool _IsInMode = false;
    private GridData _objectsData;
    private List<GameObject> _placedObjects = new List<GameObject>();
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;


    private void Start(){
        _uiPanel.gameObject.SetActive(false);
        _inputManager.OnSwitch += SwitchMode;
        StopPlacement();
        _objectsData = new GridData();
    }

    public void StartPlacement(int ID){
        StopPlacement();
        _selectedObjectIndex = _objectsDatabase._objectsData.FindIndex(data => data.ID == ID);
        
        if (_selectedObjectIndex == -1){
            Debug.Log($"Object with id: {ID} not found");
            return;
        }
        
        _gridVisualization.SetActive(true);
        _previewSystem.StartShowingPlacementPreview(
            _objectsDatabase._objectsData[_selectedObjectIndex].Prefab, 
            _objectsDatabase._objectsData[_selectedObjectIndex].Size);
        
        _inputManager.OnClicked += PlaceStructure;
        _lastDetectedPosition = Vector3Int.zero;
    }

    private void PlaceStructure(){
        if (_inputManager.IsPointerOverUI())
            return;
        
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        
        if (placementValidity == false)
            return;
        
        GameObject gameObject = Instantiate(_objectsDatabase._objectsData[_selectedObjectIndex].Prefab);
        gameObject.transform.position = _grid.CellToWorld(gridPosition);
        _placedObjects.Add(gameObject);
        GridData selectedData = _objectsData;
        selectedData.AddObjectAt(gridPosition, _objectsDatabase._objectsData[_selectedObjectIndex].Size, _objectsDatabase._objectsData[_selectedObjectIndex].ID, _placedObjects.Count - 1);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex){
        GridData selectedData = _objectsData;
        return selectedData.CanPlaceObjectAt(gridPosition, _objectsDatabase._objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement(){
        _selectedObjectIndex = -1;
        _gridVisualization.SetActive(false);
        _previewSystem.StopShowingPreview();
        _inputManager.OnClicked -= PlaceStructure;
    }

    public void SwitchMode(){
        if (!_IsInMode){
            _uiPanel.gameObject.SetActive(true);
            StartPlacement(0);
            // _previewSystem.StartShowingPlacementPreview(_objectsDatabase._objectsData[0].Prefab, _objectsDatabase._objectsData[0].Size);
        }
        else{
            _uiPanel.gameObject.SetActive(false);
            StopPlacement();
        }
        
        _IsInMode = !_IsInMode;
    }

    private void Update(){
        if (_selectedObjectIndex == -1)
            return;
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        if (_lastDetectedPosition != gridPosition){
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
            _lastDetectedPosition = gridPosition;
        }
    }
}
