using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaysWithItemsListHandler : SingletonWithDestroy<DisplaysWithItemsListHandler>
{
    [SerializeField] private List<DisplaySlotController> _displaySlotsWithItems= new List<DisplaySlotController>();

    private new void Awake()
    {
        base.Awake();
        _displaySlotsWithItems.Clear();
    }
    public void AddDisplaySlotWithItem(DisplaySlotController displaySlot)
    {
        _displaySlotsWithItems.Add(displaySlot);
    }

    public void RemoveDisplaySlotWithItem(DisplaySlotController displaySlot)
    {
        _displaySlotsWithItems.Remove(displaySlot);
    }

    public List<DisplaySlotController> GetDisplaySlotsWithItems()
    {
        return _displaySlotsWithItems;
    }
}
