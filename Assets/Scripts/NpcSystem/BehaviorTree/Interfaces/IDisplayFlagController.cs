using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplayFlagController
{
    public bool IsOccupied { get; set; }
    public bool IsChosen { get; set; }
}
