using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonBase<ResourceManager>
{
    public static GameObject Load(string path)
    {
        var obj = Instantiate(Resources.Load(path)) as GameObject;

        if (obj == null)
            Debug.Log(path + "is can't found");

        return obj;
    }
}
