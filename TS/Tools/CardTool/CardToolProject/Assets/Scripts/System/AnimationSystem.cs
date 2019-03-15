using System.Collections;
using UnityEngine;


public class AnimationSystem : SingletonBase<AnimationSystem>
{
    public IEnumerator LerpAnimation(System.Func<Vector2> getFunc, System.Action<Vector2> setAction, Vector2 dest, float speed)
    {
        float time = 0;
        Vector2 lerpVec = getFunc();
        Vector2 startVec = lerpVec;

        while (time <= 1)
        {
            time += Time.deltaTime * speed;
            lerpVec = Vector2.Lerp(startVec, dest, time);
           
            setAction(lerpVec);
            yield return null;
        }
    }

    public IEnumerator LerpRotateAnimation(System.Func<Vector3> getFunc, System.Action<Vector3> setAction, Vector3 dest,float speed)
    {
        float time = 0;
        Vector3 lerpVec = getFunc();

        while (time <= 1)
        {
            time += Time.deltaTime * speed;

            lerpVec.x = Mathf.LerpAngle(lerpVec.x, dest.x, time);
            lerpVec.y = Mathf.LerpAngle(lerpVec.y, dest.y, time);
            lerpVec.z = Mathf.LerpAngle(lerpVec.z, dest.z, time);

            setAction(lerpVec);
            yield return null;
        }
    }

    public IEnumerator FadeInAnimation(GameObject obj, float speed)
    {
        Renderer[] childRenderer = obj.GetComponentsInChildren<Renderer>();
        Color color = new Color(1, 1, 1);

        while (color.a > 0)
        {
            for (int i = 0; i < childRenderer.Length; i++)
            {
                color.a = childRenderer[i].material.color.a - Time.deltaTime * speed;
                childRenderer[i].material.color = color;
            }

            yield return null;
        }
    }

    public IEnumerator FadeInAnimation(CanvasGroup canvas, float speed)
    {
        while (canvas.alpha > 0)
        {
            canvas.alpha -= Time.deltaTime * speed;
            yield return null;
        }
    }

    public IEnumerator FadeOutAnimation(GameObject obj, float speed)
    {
        Renderer[] childRenderer = obj.GetComponentsInChildren<Renderer>();
        Color color = new Color(1, 1, 1, 0);

        while (color.a < 1)
        {
            for (int i = 0; i < childRenderer.Length; i++)
            {
                color.a = childRenderer[i].material.color.a + Time.deltaTime * speed;
                childRenderer[i].material.color = color;
            }

            yield return null;
        }
    }

    public IEnumerator FadeOutAnimation(CanvasGroup canvas, float speed)
    {
        while (canvas.alpha < 1)
        {
            canvas.alpha += Time.deltaTime * speed;
            yield return null;
        }
    }
}

