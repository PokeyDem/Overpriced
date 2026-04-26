using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData{
    private Dictionary<Vector3Int, PlacementData> _placedObjects = new Dictionary<Vector3Int, PlacementData>();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex){
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);
      
        foreach (Vector3Int pos in positionToOccupy){
            if (_placedObjects.ContainsKey(pos)){
                throw new Exception($"Dictionary already contains this cell position{pos}");
            }
            _placedObjects[pos] = data;
        }
    }

    public void ShowData(){
        foreach (var data in _placedObjects){
            Debug.Log(data.ToString());
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize){
        List<Vector3Int> returnValues = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++){
            for (int y = 0; y < objectSize.y; y++){
                returnValues.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnValues;
    }

    public void ClearPlacedObjects(){
        _placedObjects = new Dictionary<Vector3Int, PlacementData>();
    }
    
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, Vector3Int playerPosition){
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy){
            if (_placedObjects.ContainsKey(pos) || (pos.x == playerPosition.x && pos.z == playerPosition.z)){
                return false;
            }
        }
        return true;
    }
}

public class PlacementData{
    public List<Vector3Int> OccupiedPositions;
    public int ID{ get; private set; }
    public int PlacedObjectIndex  { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int placedObjectIndex){
        this.OccupiedPositions = occupiedPositions;
        this.ID = ID;
        this.PlacedObjectIndex = placedObjectIndex;
    }
}
