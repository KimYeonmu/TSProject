using UnityEngine;
using System.Collections;

public static class G
{
    private static SceneController _sceneController = new SceneController();
    public static SceneController SceneController
    {
        get { return _sceneController; }
    }
}
