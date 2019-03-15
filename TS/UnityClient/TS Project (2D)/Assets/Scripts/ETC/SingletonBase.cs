using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType (typeof(T)) as T;

            if (_instance == null)
            {
                return null;
            }
        }

        return _instance;
    }
}

