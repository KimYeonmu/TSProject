using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardAnimationSystem : SingletonBase<CardAnimationSystem>
{
    public IEnumerator ReverseAnimation(Card card, float speed)
    {
        float time = 0;
        bool isReverseAnim = false;

        Vector3 cardScale = card.GetScale();
        Vector3 startScale = cardScale;
        Vector3 destScale = new Vector3(-cardScale.x,cardScale.y);

        while (time < 1)
        {
            time += speed * Time.deltaTime;

            cardScale = Vector3.Lerp(startScale, destScale, time);
            card.SetScale(cardScale);

            if (time > 0.5f && isReverseAnim == false)
            {
                isReverseAnim = true;
                card.IsShapesActive();
                card.IsTextsActive();

                card.IsBack = !card.IsBack;
                card.SetBackgroundSprite(CardSystem.GetInstance().GetCardSprite(card.IsBack));
            }

            yield return null;
        }

        yield return null;
    }
}

