using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public abstract string GetSceneName();

    IEnumerable ProcessShow()
    {
        yield return null;
    }

    IEnumerable ProcessHide()
    {
        yield return null;
    }
}
