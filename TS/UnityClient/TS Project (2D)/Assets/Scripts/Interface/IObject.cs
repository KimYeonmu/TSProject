using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IObject : MonoBehaviour
{
    public Transform ObjTransform;

    public void Awake()
    {
        ObjTransform = gameObject.transform;
    }

    #region Setter

    public void SetParent(Transform parent){ObjTransform.parent = parent; }

    public void SetReverse()
    {
        this.SetScale(new Vector3(
            ObjTransform.localScale.x,
            -ObjTransform.localScale.y,
            ObjTransform.localScale.z));
    }

    public void SetPosition(Vector2 position) { ObjTransform.position = position; }

    public void SetLocalPosition(Vector2 position) { ObjTransform.position = position; }

    public void SetScale(Vector2 scale) { ObjTransform.localScale = scale; }

    public void SetRotation(Vector3 angle) { ObjTransform.localEulerAngles = angle; }

    #endregion

    #region Getter

    public Vector2 GetLocalPosition() { return ObjTransform.localPosition; }

    public Vector2 GetPosition() { return ObjTransform.position; }

    public Vector2 GetScale() { return ObjTransform.localScale; }

    public Vector3 GetRotation() { return ObjTransform.localEulerAngles; }

    public Transform GetParent() { return ObjTransform.parent; }

    #endregion getter

}
