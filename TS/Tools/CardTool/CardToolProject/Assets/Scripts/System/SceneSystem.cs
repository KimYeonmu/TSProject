using System.Collections;
using UnityEngine;

public enum SceneEventTag
{
    SCENE_EVENT_IN = 0,
    SCENE_EVENT_OUT,
    SCENE_EVENT_SCALE_DOWN
}

public class SceneSystem : SingletonBase<SceneSystem>
{
    public CanvasGroup PlaySceneUiGroup;
    public RectTransform PlaySceneUi;
    public GameObject RootObject;

    public Vector2 ScreenSize;
    public Vector2 ScreenPoint;
    public Vector2 ScreenWorldPoint;


    public void Awake()
    {
        ScreenSize.x = GameObject.Find("BackGround").GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        ScreenSize.y = GameObject.Find("BackGround").GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        ScreenPoint.x = Camera.main.pixelWidth;
        ScreenPoint.y = Camera.main.pixelHeight;

        ScreenWorldPoint = Camera.main.ScreenToWorldPoint(ScreenPoint);
    }

    public void SceneEvent(SceneEventTag sceneEventTag)
    {
        switch (sceneEventTag)
        {
            case SceneEventTag.SCENE_EVENT_IN:
                break;
            case SceneEventTag.SCENE_EVENT_OUT:
                {
                    StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                        this.GetUiPosition,
                        this.SetUiPosition,
                        Vector2.zero, 2));

                    StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                        () => { return RootObject.transform.localPosition; },
                        (Vector2 vc) => { RootObject.transform.localPosition = vc;},
                        Vector3.zero, 2));

                    
                    StartCoroutine(AnimationSystem.GetInstance().FadeOutAnimation(
                        RootObject, 1));

                    StartCoroutine(AnimationSystem.GetInstance().FadeOutAnimation(
                        PlaySceneUiGroup, 1));
                }
                break;
            case SceneEventTag.SCENE_EVENT_SCALE_DOWN:
                StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                        this.GetUiScale,
                        this.SetUiScale,
                        Vector2.one, 2));

                StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                        () => { return RootObject.transform.localScale; },
                        (Vector2 vc) => { RootObject.transform.localScale = vc; },
                        Vector3.one*100, 2));
                break;
        }
    }

    public void SetSceneScale(Vector2 scale)
    {
        PlaySceneUi.localScale = scale;
        RootObject.transform.localScale = scale;
    }

    public void SetScenePos(Vector2 pos)
    {
        PlaySceneUi.localPosition = pos;
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

        PlaySceneUiGroup.alpha = alpha;
    }

    public void SetUiPosition(Vector2 position)
    {
        PlaySceneUi.localPosition = position;
    }

    public void SetUiScale(Vector2 scale)
    {
        PlaySceneUi.localScale = scale;
    }

    public Vector2 GetUiPosition()
    {
        return PlaySceneUi.localPosition;
    }

    public Vector2 GetUiScale()
    {
        return PlaySceneUi.localScale;
    }
}

