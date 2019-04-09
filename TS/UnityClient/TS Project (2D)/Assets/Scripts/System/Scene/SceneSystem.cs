using System.Collections;
using DG.Tweening;
using UnityEngine;

public enum SceneEventTag
{
    SCENE_EVENT_IN = 0,
    SCENE_EVENT_OUT,
    SCENE_EVENT_SCALE_DOWN
}

public class SceneSystem : SingletonBase<SceneSystem>
{
    [HideInInspector] public IScene NowScene;
    [HideInInspector] public CanvasGroup SceneUiGroup;
    [HideInInspector] public RectTransform SceneUi;
    [HideInInspector] public GameObject RootObject;

    [HideInInspector] public Vector2 ScreenSize;
    [HideInInspector] public Vector2 ScreenPoint;
    [HideInInspector] public Vector2 ScreenWorldPoint;

    public void Awake()
    {
        //DontDestroyOnLoad(transform);

        ScreenPoint.x = Camera.main.pixelWidth;
        ScreenPoint.y = Camera.main.pixelHeight;

        ScreenWorldPoint = Camera.main.ScreenToWorldPoint(ScreenPoint);
    }

    public void SetScreenSize(Vector2 vc)
    {
        ScreenSize = vc;
    }

    public void SceneEvent(SceneEventTag sceneEventTag)
    {
        switch (sceneEventTag)
        {
            case SceneEventTag.SCENE_EVENT_SCALE_DOWN:
                {
                    SceneUi.transform.DOScale(Vector3.one, 0.1f);
                    RootObject.transform.DOScale(Vector3.one, 0.1f);
                }
                break;
        }
    }

    public void SetSceneScale(Vector2 scale)
    {
        SceneUi.localScale = scale;
        RootObject.transform.localScale = scale;
    }

    public void SetScenePos(Vector2 pos)
    {
        SceneUi.localPosition = pos;
        RootObject.transform.localPosition = pos;
    }

    public void SetSceneAlpha(float alpha)
    {
        Renderer[] childRenderer = RootObject.GetComponentsInChildren<Renderer>();
        Color color = new Color(1, 1, 1, alpha);

        for (int i = 0; i < childRenderer.Length; i++)
        {
            childRenderer[i].material.color = color;
        }

        SceneUiGroup.alpha = alpha;
    }

    public void SetUiPosition(Vector2 position)
    {
        SceneUi.localPosition = position;
    }

    public void SetUiScale(Vector2 scale)
    {
        SceneUi.localScale = scale;
    }

    public Vector2 GetUiPosition()
    {
        return SceneUi.localPosition;
    }

    public Vector2 GetUiScale()
    {
        return SceneUi.localScale;
    }
}

