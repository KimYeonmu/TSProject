using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObject : MonoBehaviour
{
    public GameObject Object;

	// Use this for initialization
	public void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

    public void SetParent(Transform trans)
    {
        Object.transform.parent = trans;
    }

    public void SetReverse()
    {
        this.SetScale(new Vector3(
            Object.transform.localScale.x,
            -Object.transform.localScale.y,
            Object.transform.localScale.z));
    }

    public void SetPosition(Vector2 position)
    {
        Object.transform.localPosition = position;
    }

    public void SetWorldPosition(Vector2 position)
    {
        Object.transform.position = position;
    }

    public void SetScale(Vector2 scale)
    {
        Object.transform.localScale = scale;
    }

    public void SetRotation(Vector3 angle)
    {
        Object.transform.localEulerAngles = angle;
    }

    public Vector2 GetPosition()
    {
        return Object.transform.localPosition;
    }

    public Vector2 GetScale()
    {
        return Object.transform.localScale;
    }

    public Vector3 GetRotation()
    {
        return Object.transform.localEulerAngles;
    }

    public Transform GetParent()
    {
        return Object.transform.parent;
    }
}
