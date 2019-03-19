using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonBase<ResourceManager>
{
    public Transform RootResourceObj;

    public GameObject Load(string path)
    {
        var obj = Instantiate(Resources.Load(path)) as GameObject;

        obj.transform.SetParent(RootResourceObj);
        obj.transform.localPosition = Vector3.zero;

        if (obj == null)
            Debug.Log(path + "is can't found");

        return obj;
    }
}
