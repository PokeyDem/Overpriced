using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILinePositionManager
{
    public bool GetPositionInLineOccupancy(int i);
    public void SetPositionInLineOccupancy(int i, bool isOccupied);
}
