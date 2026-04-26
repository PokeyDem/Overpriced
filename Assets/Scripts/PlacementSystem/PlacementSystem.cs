using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabaseSO objectsDatabase;
    [SerializeField] private ItemsDatabaseSO itemsDatabase;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private Canvas uiPanel;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private GameObject player;
    [SerializeField] private List<PreplacedStructure> preplacedStructures = new List<PreplacedStructure>();
    
    private Vector3 _playerPosition;
    private Vector3Int _playerGridPosition;
    private int _selectedObjectIndex = -1;
    private bool _isInEditMode = false;
    private GridData _gridData;
    private List<DisplaySlotController> _placedObjects = new List<DisplaySlotController>();
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    private void Start(){
        uiPanel.gameObject.SetActive(false);
        inputManager.OnSwitch += SwitchMode;
        StopPlacement();
        _gridData = new GridData();

        foreach (var structure in preplacedStructures){
            PreplaceStructure(structure);
        }
    }

    public void StartPlacement(int ID){
        StopPlacement();
        _selectedObjectIndex = objectsDatabase.objectsData.FindIndex(data => data.ID == ID);
        
        if (_selectedObjectIndex == -1){
            Debug.Log($"Object with id: {ID} not found");
            return;
        }
        
        gridVisualization.SetActive(true);
        previewSystem.StartShowingPlacementPreview(
            objectsDatabase.objectsData[_selectedObjectIndex].Prefab, 
            objectsDatabase.objectsData[_selectedObjectIndex].Size);
        inputManager.OnRmbClick += PlaceStructure;
        _lastDetectedPosition = Vector3Int.zero;
    }

    private void PlaceStructure(){
        if (inputManager.IsPointerOverUI())
            return;
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        
        if (placementValidity == false)
            return;
        
        GameObject placedObject = Instantiate(objectsDatabase.objectsData[_selectedObjectIndex].Prefab);
        placedObject.transform.position = grid.CellToWorld(gridPosition);
        
        _placedObjects.Add(placedObject.GetComponentInChildren<DisplaySlotController>());
        _placedObjects[_placedObjects.Count - 1].SetDisplayTypeId(_selectedObjectIndex);
        _placedObjects[_placedObjects.Count - 1].SetPosition(placedObject.transform.position);
        
        GridData selectedData = _gridData;
        selectedData.AddObjectAt(gridPosition, objectsDatabase.objectsData[_selectedObjectIndex].Size, objectsDatabase.objectsData[_selectedObjectIndex].ID, _placedObjects.Count - 1);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private void PreplaceStructure(PreplacedStructure preplacedStructure){
        
        GameObject preplacedObject = Instantiate(objectsDatabase.objectsData[preplacedStructure.Index].Prefab);
        preplacedObject.transform.position = grid.CellToWorld(preplacedStructure.GetPos());
        
        _placedObjects.Add(preplacedObject.GetComponentInChildren<DisplaySlotController>());
        _placedObjects[_placedObjects.Count - 1].SetDisplayTypeId(preplacedStructure.Index);
        _placedObjects[_placedObjects.Count - 1].SetPosition(preplacedObject.transform.position);
        
        GridData selectedData = _gridData;
        selectedData.AddObjectAt(preplacedStructure.GetPos(), 
            objectsDatabase.objectsData[preplacedStructure.Index].Size,
            objectsDatabase.objectsData[preplacedStructure.Index].ID, 
            _placedObjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex){
        GridData selectedData = _gridData;
        return selectedData.CanPlaceObjectAt(gridPosition, objectsDatabase.objectsData[selectedObjectIndex].Size, _playerGridPosition);
    }

    private void StopPlacement(){
        _selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        previewSystem.StopShowingPreview();
        inputManager.OnRmbClick -= PlaceStructure;
    }

    public void SwitchMode(){
        if (!_isInEditMode){
            uiPanel.gameObject.SetActive(true);
            StartPlacement(0);
        }
        else{
            uiPanel.gameObject.SetActive(false);
            StopPlacement();
        }
        
        _isInEditMode = !_isInEditMode;
    }

    private void Update(){
        if (_selectedObjectIndex == -1)
            return;
        _playerPosition = player.transform.position;
        _playerGridPosition = grid.WorldToCell(_playerPosition);
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (_lastDetectedPosition != gridPosition){
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            _lastDetectedPosition = gridPosition;
        }
    }
}

[Serializable]
public class PreplacedStructure{
    [field: SerializeField, Tooltip("Position on the grid(x,y)")] private Vector2Int Pos{ get; set;}

    [field: SerializeField, Tooltip("Prefab index in database")] public int Index{ get; private set; }
    
    [field:SerializeField, Tooltip("Elevation level on the Y axis")] private int gridElevation = -1;

    public Vector3Int GetPos(){
        return new Vector3Int(Pos.x, gridElevation, Pos.y);
    }
    
}