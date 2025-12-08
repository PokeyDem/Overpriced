using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[Serializable] 
public class Talent { 
        public string name;
        public string description;
        public Sprite icon; 
        public UnityEvent OnUnlock;
}
