using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController<T> : MonoBehaviour
where T : UnityEngine.Object
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<T>();
            return _instance;
        }
    }
}