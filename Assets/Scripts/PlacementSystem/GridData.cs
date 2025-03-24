using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData{
    private Dictionary<Vector3Int, PlacementData> placedObject = new Dictionary<Vector3Int, PlacementData>();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex){
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
      
        foreach (Vector3Int pos in positionToOccupy){
            if (placedObject.ContainsKey(pos)){
                throw new Exception($"Dictionary already contains this cell position{pos}");
            }
            placedObject[pos] = data;
        }
    }

    public void ShowData(){
        foreach (var VARIABLE in placedObject){
            Debug.Log(VARIABLE.ToString());
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
        placedObject = new Dictionary<Vector3Int, PlacementData>();
    }
    
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, Vector3Int _playerPosition){
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy){
            if (placedObject.ContainsKey(pos) || (pos.x == _playerPosition.x && pos.z == _playerPosition.z)){
                return false;
            }
        }
        return true;
    }
}

public class PlacementData{
    public List<Vector3Int> occupiedPositions;
    public int ID{ get; private set; }
    public int placedObjectIndex  { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int placedObjectIndex){
        this.occupiedPositions = occupiedPositions;
        this.ID = ID;
        this.placedObjectIndex = placedObjectIndex;
    }
}
