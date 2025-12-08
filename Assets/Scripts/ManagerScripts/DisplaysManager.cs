using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaysManager : SingletonWithDestroy<DisplaysManager>
{
    [SerializeField] private DisplaySlotController[] _slots;
    [SerializeField] private DisplaySlotController[] _purchasableSlots;


    private new void Awake()
    {
        base.Awake();
    }

    public void ResetDisplays()
    {
        foreach (var slot in _slots)
        {
            slot.RemoveItem();
        }

        foreach (var slot in _purchasableSlots)
        {
            slot.RemoveItem();
            slot.ResetPurchasableDisplay();
        }
    }
}
