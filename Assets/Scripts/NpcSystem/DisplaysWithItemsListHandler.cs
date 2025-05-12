using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisplaysWithItemsListHandler : SingletonWithDestroy<DisplaysWithItemsListHandler>
{
    [SerializeField] private List<DisplaySlotController> _displaySlotsWithItems= new List<DisplaySlotController>();

    public UnityEvent itemPlaced;

    private new void Awake()
    {
        base.Awake();
        _displaySlotsWithItems.Clear();
    }
    public void AddDisplaySlotWithItem(DisplaySlotController displaySlot)
    {
        _displaySlotsWithItems.Add(displaySlot);
        itemPlaced?.Invoke();
        //SwapLastWithRandom();
    }

    public void RemoveDisplaySlotWithItem(DisplaySlotController displaySlot)
    {
        _displaySlotsWithItems.Remove(displaySlot);
    }

    public List<DisplaySlotController> GetDisplaySlotsWithItems()
    {
        Shuffle();
        return _displaySlotsWithItems;
    }

    void SwapLastWithRandom()
    {
        int random = Random.Range(0, _displaySlotsWithItems.Count);
        DisplaySlotController temp = _displaySlotsWithItems[random];
        _displaySlotsWithItems[random] = _displaySlotsWithItems[_displaySlotsWithItems.Count - 1];
        _displaySlotsWithItems[_displaySlotsWithItems.Count - 1] = temp;
    }
    void Shuffle()
    {
        int n = _displaySlotsWithItems.Count;
        while (n > 1)
        {
            n--;
            int random = Random.Range(0, n+1);
            DisplaySlotController value = _displaySlotsWithItems[random];
            _displaySlotsWithItems[random] = _displaySlotsWithItems[n];
            _displaySlotsWithItems[n] = value;
        }
    }
}
