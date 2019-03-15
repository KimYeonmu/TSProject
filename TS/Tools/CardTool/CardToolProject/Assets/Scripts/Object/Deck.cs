using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Deck : IObject
{
    public List<Card> DeckList = new List<Card>();

    public void Awake()
    {
        Object = new GameObject();
        Object.name = "Deck";
    }

    public void Start()
    {
    }

    public void AddCard(Card card)
    {
        card.SetSortingOrder(DeckList.Count);
        card.SetParent(Object.transform);

        StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
            card.GetPosition,
            card.SetPosition,
            GetPosition(),
            5));

        DeckList.Add(card);
    }

    public Card GetTopCard()
    {
        Card card = DeckList[DeckList.Count - 1];
        card.Object.SetActive(true);
        DeckList.RemoveAt(DeckList.Count - 1);
        return card;
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
}