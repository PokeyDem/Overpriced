using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonDontDestroyOnLoad<T> : MonoBehaviour
    where T : Component
{
    public static T Instance { get; private set; }



    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
        DontDestroyOnLoad(this);
    }
}
