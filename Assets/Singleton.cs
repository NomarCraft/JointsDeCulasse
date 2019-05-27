﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] instances = FindObjectsOfType<T>();
                if (instances.Length > 1)
                {
                    Debug.LogError("Warning, there is more than One Singleton");
                }
                if (instances != null && instances.Length > 0)
                {
                    _instance = instances[0];
                }
            }
            return _instance;
        }
    }
}

