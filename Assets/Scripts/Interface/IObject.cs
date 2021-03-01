using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>오브젝트 인터페이스 </summary>
public class IObject : MonoBehaviour
{
    public Transform ObjTransform;

    public void Awake()
    {
        ObjTransform = gameObject.transform;
    }

    #region Setter

    public void SetParent(Transform parent){ObjTransform.parent = parent; }

    /// <summary>오브젝트를 y축으로 뒤집는 함수 </summary>
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

    public Vector3 GetAngle() { return ObjTransform.eulerAngles; }

    public Vector3 GetLocalAngle() { return ObjTransform.localEulerAngles; }

    public Transform GetParent() { return ObjTransform.parent; }

    #endregion getter
}
