using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour{
    [SerializeField] InputManager _inputManager;
    [SerializeField] Grid _grid;
    [SerializeField] ObjectsDatabaseSO _objectsDatabase;
    [SerializeField] ItemsDatabaseSO _itemsDatabase;
    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private Canvas _uiPanel;
    [SerializeField] PreviewSystem _previewSystem;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<PreplacedStructure> _preplacedStructures = new List<PreplacedStructure>();
    private Vector3 _playerPosition;
    private Vector3Int _playerGridPosition;
    private int _selectedObjectIndex = -1;
    private bool _IsInEditMode = false;
    private GridData _objectsData;
    private List<DisplaySlotController> _placedObjects = new List<DisplaySlotController>();
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    private void Start(){
        _uiPanel.gameObject.SetActive(false);
        _inputManager.OnSwitch += SwitchMode;
        StopPlacement();
        _objectsData = new GridData();

        foreach (var structure in _preplacedStructures){
            PreplaceStructure(structure);
        }
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
        
        _placedObjects.Add(gameObject.GetComponentInChildren<DisplaySlotController>());
        _placedObjects[_placedObjects.Count - 1].SetDisplayTypeId(_selectedObjectIndex);
        _placedObjects[_placedObjects.Count - 1].SetPosition(gameObject.transform.position);
        
        GridData selectedData = _objectsData;
        selectedData.AddObjectAt(gridPosition, _objectsDatabase._objectsData[_selectedObjectIndex].Size, _objectsDatabase._objectsData[_selectedObjectIndex].ID, _placedObjects.Count - 1);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    private void PreplaceStructure(PreplacedStructure _preplacedStructure){
        
        GameObject gameObject = Instantiate(_objectsDatabase._objectsData[_preplacedStructure._index].Prefab);
        gameObject.transform.position = _grid.CellToWorld(_preplacedStructure.GetPos());
        
        _placedObjects.Add(gameObject.GetComponentInChildren<DisplaySlotController>());
        _placedObjects[_placedObjects.Count - 1].SetDisplayTypeId(_preplacedStructure._index);
        _placedObjects[_placedObjects.Count - 1].SetPosition(gameObject.transform.position);
        
        GridData selectedData = _objectsData;
        selectedData.AddObjectAt(_preplacedStructure.GetPos(), 
            _objectsDatabase._objectsData[_preplacedStructure._index].Size,
            _objectsDatabase._objectsData[_preplacedStructure._index].ID, 
            _placedObjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex){
        GridData selectedData = _objectsData;
        return selectedData.CanPlaceObjectAt(gridPosition, _objectsDatabase._objectsData[selectedObjectIndex].Size, _playerGridPosition);
    }

    private void StopPlacement(){
        _selectedObjectIndex = -1;
        _gridVisualization.SetActive(false);
        _previewSystem.StopShowingPreview();
        _inputManager.OnClicked -= PlaceStructure;
    }

    public void SwitchMode(){
        if (!_IsInEditMode){
            _uiPanel.gameObject.SetActive(true);
            StartPlacement(0);
            // _previewSystem.StartShowingPlacementPreview(_objectsDatabase._objectsData[0].Prefab, _objectsDatabase._objectsData[0].Size);
        }
        else{
            _uiPanel.gameObject.SetActive(false);
            StopPlacement();
        }
        
        _IsInEditMode = !_IsInEditMode;
    }

    private void Update(){
        if (_selectedObjectIndex == -1)
            return;
        _playerPosition = _player.transform.position;
        _playerGridPosition = _grid.WorldToCell(_playerPosition);
        Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        if (_lastDetectedPosition != gridPosition){
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
            _lastDetectedPosition = gridPosition;
        }
    }

    public List<DisplayData> GetDisplayData(){
        List<DisplayData> displayData = new List<DisplayData>();
        Vector3 pos;
        foreach (DisplaySlotController display in _placedObjects){
            DisplaySlotController[] slots = display.transform.parent.GetComponentsInChildren<DisplaySlotController>();
            List<int> itemIds = new List<int>();

            foreach (var displayController in slots){
                itemIds.Add(displayController.GetItemId());
            }
            
            pos = display.GetPosition();
            displayData.Add(new DisplayData(display.GetDisplayTypeId(), pos.x, pos.y, pos.z, itemIds));
        }

        return displayData;
    }

    public void LoadDisplayData(List<DisplayData> displayData){
        _placedObjects.Clear();
        _objectsData.ClearPlacedObjects();
        _objectsData.ShowData();
        

        foreach (var display in GameObject.FindGameObjectsWithTag("Display")){
            Destroy(display);
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("Item")){
            Destroy(item);
        }
        
        foreach (DisplayData data in displayData){
            GameObject gameObject = Instantiate(_objectsDatabase._objectsData[data.displayId].Prefab);
            gameObject.transform.position = _grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(data.x),
                Mathf.RoundToInt(data.y),
                Mathf.RoundToInt(data.z -0.5f)));
        
            _placedObjects.Add(gameObject.GetComponentInChildren<DisplaySlotController>());
            _placedObjects[_placedObjects.Count - 1].SetDisplayTypeId(data.displayId);
            _placedObjects[_placedObjects.Count - 1].SetPosition(gameObject.transform.position);

            List<int> itemsId = data.itemsID;
            DisplaySlotController[] displaySlotControllers = _placedObjects[_placedObjects.Count - 1].gameObject
                .transform.parent.GetComponentsInChildren<DisplaySlotController>();
            
            if (itemsId.Count > 1){
                int counter = 0;
                
                for (int i = 0; i < displaySlotControllers.Length; i++){
                    if (itemsId[i] != -1) 
                        displaySlotControllers[i].PlaceItem(_itemsDatabase._itemsData[itemsId[i]]);
                }
                
               
            }
            else if (itemsId.Count == 1){ 
                if (itemsId[0] != -1) 
                    displaySlotControllers[0].PlaceItem(_itemsDatabase._itemsData[itemsId[0]]);
            }
                

            // if (data.itemID != -1){
            //     _placedObjects[_placedObjects.Count - 1].PlaceItem(_itemsDatabase._itemsData[data.itemID].Prefab, data.itemID);
            // }
        
            GridData selectedData = _objectsData;
            selectedData.AddObjectAt(new Vector3Int(    Mathf.RoundToInt(data.x),
                    Mathf.RoundToInt(data.y),
                    Mathf.RoundToInt(data.z)), 
                _objectsDatabase._objectsData[data.displayId].Size,
                _objectsDatabase._objectsData[data.displayId].ID, 
                _placedObjects.Count - 1);
            
        }
    }
}

[Serializable]
public class PreplacedStructure{
    [field: SerializeField, Tooltip("Position on the grid(x,y)")] private Vector2Int _pos{ get; set;}

    [field: SerializeField, Tooltip("Prefab index in database")] public int _index{ get; private set; }

    public Vector3Int GetPos(){
        return new Vector3Int(_pos.x, -1, _pos.y);
    }
}