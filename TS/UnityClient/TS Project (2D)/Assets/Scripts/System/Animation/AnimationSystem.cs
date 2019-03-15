using System.Collections;
using UnityEngine;


public class AnimationSystem : SingletonBase<AnimationSystem>
{
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

