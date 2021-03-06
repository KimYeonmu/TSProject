﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UniRx;
using UnityEngine;


public class Deck : IObject
{
    public List<Card> DeckList = new List<Card>();

    public new void Awake()
    {
        base.Awake();

        gameObject.name = "Deck";
    }

    public void Start()
    {
    }

    public void AddCard(Card card)
    {
        card.SetSortingOrder(DeckList.Count);
        card.SetParent(gameObject.transform);
        DeckList.Add(card);
    }

    public Card GetTopCard()
    {
        if (DeckList.Count <= 0)
            return null;

        Card card = DeckList[DeckList.Count - 1];
        DeckList.RemoveAt(DeckList.Count - 1);
        return card;
    }

    public Card GetTopCardPeek()
    {
        if (DeckList.Count <= 0)
            return null;

        return DeckList[DeckList.Count - 1];
    }

    public void ShuffleDeck(int nShuffle)
    {
        for (int i = 0; i < nShuffle; i++)
        {
            int rand1 = UnityEngine.Random.Range(0, DeckList.Count);
            int rand2 = UnityEngine.Random.Range(0, DeckList.Count);

            SwapCard(rand1, rand2);
        }
    }

    public void SwapCard(int random1, int random2)
    {
        DeckList[random1].SetSortingOrder(random2);
        DeckList[random2].SetSortingOrder(random1);

        Card temp = DeckList[random1];
        DeckList[random1] = DeckList[random2];
        DeckList[random2] = temp;
    }

    public void AllMoveCardDecktoDeck(Deck moveDeck, float intervalTime, float moveTime, Action complete = null)
    {
        Observable.Interval(TimeSpan.FromSeconds(intervalTime))
            .Take(DeckList.Count)
            .Subscribe(_ =>
                {
                    int i = (int)_;

                    DeckList[i].SetSortingOrder(i);
                    DeckList[i].ObjTransform.DOMove(moveDeck.GetPosition(), moveTime);

                    CardAnimationSystem.GetInstance().ReverseAnimation(
                        DeckList[i],
                        moveTime);

                    moveDeck.AddCard(DeckList[i]);
                },
                () =>
                {
                    DeckList.Clear();
                    if (complete != null )
                        complete();
                });
    }

    public void MoveCardDecktoDeck(Deck moveDeck, int cardNum, float intervalTime, float moveTime)
    {
        Observable.Interval(TimeSpan.FromSeconds(intervalTime))
            .Take(cardNum)
            .Subscribe(_ =>
            {
                moveDeck.AddCard(DeckList[DeckList.Count - 1]);

                DeckList[DeckList.Count - 1].transform.DOMove(moveDeck.GetPosition(), moveTime);

                CardAnimationSystem.GetInstance().ReverseAnimation(
                    DeckList[DeckList.Count - 1],
                    moveTime);

                DeckList.RemoveAt(DeckList.Count - 1);
            });
    }

    public void MoveCardDecktoDeck(Deck moveDeck, int startCardIdx, int cardNum, float intervalTime, float moveTime, Action complete = null)
    {
        Observable.Interval(TimeSpan.FromSeconds(intervalTime))
            .Take(cardNum)
            .Subscribe(i =>
            {
                int idx = startCardIdx;
                moveDeck.AddCard(DeckList[idx]);

                DeckList[idx].transform.DOMove(moveDeck.GetPosition(), moveTime);

                CardAnimationSystem.GetInstance().ReverseAnimation(
                    DeckList[idx],
                    moveTime);

                DeckList.RemoveAt(idx);
            }, () =>
            {
                for (int i = 0; i < DeckList.Count; i ++)
                    DeckList[i].SetSortingOrder(i);

                if (complete != null)
                    complete();
            });
    }

}