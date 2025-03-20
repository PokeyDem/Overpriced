using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonWithDestroy<T> : MonoBehaviour 
    where T : Component {
    public static T Instance { get; private set; }
    protected void Awake()
    {
        if (Instance !=null)
        {
            Destroy(this);
            return;
        }
        Instance = this as T;
    }
}
