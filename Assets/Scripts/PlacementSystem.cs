using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour{
    [SerializeField] GameObject _cellIndicator;
    [SerializeField] InputManager _inputManager;
    [SerializeField] Grid _grid;
    [SerializeField] ObjectsDatabaseSO _objectsDatabase;
    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private Canvas _uiPanel;
    private int _selectedObjectIndex = -1;
    private bool _IsInMode = false;


    private void Start(){
        _uiPanel.gameObject.SetActive(false);
        _inputManager.OnSwitch += SwitchMode;
        StopPlacement();
    }

    public void StartPlacement(int ID){
        StopPlacement();
        _selectedObjectIndex = _objectsDatabase._objectsData.FindIndex(data => data.ID == ID);
        
        if (_selectedObjectIndex == -1){
            Debug.Log($"Object with id: {ID} not found");
            return;
        }
        
        _gridVisualization.SetActive(true);
        _cellIndicator.SetActive(true);
        _inputManager.OnClicked += PlaceStructure;
    }

    private void PlaceStructure(){
        if (_inputManager.IsPointerOverUI())
            return;
        
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
        GameObject gameObject = Instantiate(_objectsDatabase._objectsData[_selectedObjectIndex].Prefab);
        gameObject.transform.position = _grid.CellToWorld(gridPosition);
        
    }

    private void StopPlacement(){
        _selectedObjectIndex = -1;
        _gridVisualization.SetActive(false);
        _cellIndicator.SetActive(false);
        _inputManager.OnClicked -= PlaceStructure;
    }

    public void SwitchMode(){
        if (!_IsInMode){
            _uiPanel.gameObject.SetActive(true);
            StartPlacement(0);
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
        _cellIndicator.transform.position = _grid.CellToWorld(gridPosition);
    }
}
