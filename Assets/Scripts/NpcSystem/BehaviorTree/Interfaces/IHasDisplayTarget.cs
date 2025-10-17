using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasDisplayTarget
{
    public DisplaySlotController DisplayTarget { get; set; }
}
