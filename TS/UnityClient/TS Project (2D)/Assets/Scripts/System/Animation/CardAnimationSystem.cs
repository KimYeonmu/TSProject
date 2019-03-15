using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
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

    public IEnumerator SinAnimation(List<Card> cards)
    {
        WaitForSeconds delay = new WaitForSeconds(5);

        yield return delay;

        delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.DOMove(new Vector3(
                    Mathf.Sin((360 / cards.Count + 1) * i * Mathf.Deg2Rad) * 5,
                    Mathf.Cos((360 / cards.Count + 1) * i * Mathf.Deg2Rad) * 5), 5);

            yield return delay;
        }

        delay = new WaitForSeconds(2);
        yield return delay;

        StartCoroutine(CardSystem.GetInstance().AllCardReverse());
    }
}

