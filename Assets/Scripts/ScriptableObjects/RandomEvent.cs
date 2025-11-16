using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class RandomEvent
    {
        [field:SerializeField] public string eventName;
        [field:SerializeField] public string eventDescription;
        [field:SerializeField] public float modifier;
        [field:SerializeField] public ItemType itemType;
    }
}