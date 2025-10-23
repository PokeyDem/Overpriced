using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplayInfoProvider
{
    public Vector3 Position{ get; }
    public ItemData ItemData{ get; }
}
