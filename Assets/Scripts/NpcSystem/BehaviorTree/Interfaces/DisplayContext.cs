using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayContext
{
    public IDisplayInfoProvider Info { get; }
    public IDisplayFlagController Flags { get; }
    public IDisplayEditor Editor { get; }

    public DisplayContext(IDisplayInfoProvider info, IDisplayFlagController flags, IDisplayEditor editor)
    {
        Info = info;
        Flags = flags;
        Editor = editor;
    }
}
