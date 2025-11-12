using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisplaysWithItemsListHandler : SingletonWithDestroy<DisplaysWithItemsListHandler>
{
    [SerializeField] private List<DisplayContext> _displaySlotsWithItems= new List<DisplayContext>();

    public UnityEvent itemPlaced;
    public UnityEvent listEmpty;

    private new void Awake()
    {
        base.Awake();
        _displaySlotsWithItems.Clear();
        listEmpty?.Invoke();
    }
    private void Start()
    {
        listEmpty?.Invoke();
    }
    public void AddDisplaySlotWithItem(DisplayContext displaySlot)
    {
        DisplayContext displaySlotContext =displaySlot;
        _displaySlotsWithItems.Add(displaySlotContext);
        itemPlaced?.Invoke();
        //SwapLastWithRandom();
    }

    public void RemoveDisplaySlotWithItem(DisplayContext displaySlot)
    {
        _displaySlotsWithItems.Remove(displaySlot);
        if (_displaySlotsWithItems != null || _displaySlotsWithItems.Count == 0)
        {
            listEmpty?.Invoke();
        }
    }

    public List<DisplayContext> GetDisplaySlotsWithItems()
    {
        Shuffle();
        return _displaySlotsWithItems;
    }

    void SwapLastWithRandom()
    {
        int random = Random.Range(0, _displaySlotsWithItems.Count);
        DisplayContext temp = _displaySlotsWithItems[random];
        _displaySlotsWithItems[random] = _displaySlotsWithItems[_displaySlotsWithItems.Count - 1];
        _displaySlotsWithItems[_displaySlotsWithItems.Count - 1] = temp;
    }
    void Shuffle()
    {
        for (int i = _displaySlotsWithItems.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            var temp = _displaySlotsWithItems[i];
            _displaySlotsWithItems[i] = _displaySlotsWithItems[randomIndex];
            _displaySlotsWithItems[randomIndex] = temp;
        }
    }
    public void CheckListEmpty()
    {
        listEmpty?.Invoke();
    }
}
