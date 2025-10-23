using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasDisplayTarget
{
    public DisplayContext DisplayTarget { get; set; }
}
