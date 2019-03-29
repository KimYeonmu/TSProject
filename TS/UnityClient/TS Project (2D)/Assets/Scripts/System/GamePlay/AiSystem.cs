using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class AiSystem : SingletonBase<AiSystem>
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool RandomCard(int playerIndex)
    {
        var player = PlayerSystem.GetInstance().Players[playerIndex];

        var cardIndexs = new List<int>();

        for (int i = 0; i < player.PlayerCard.Count; i++)
        {
            if (RuleSystem.GetInstance().CompareCard(
                DeckSystem.GetInstance().GetTopCardPeekWithDeck(DeckTag.PUT_DECK),
                player.PlayerCard[i],
                player.IsPutCard))
            {
                cardIndexs.Add(i);
            }
        }

        if (cardIndexs.Count <= 0)
            return false;

        var rand = new Random();

        var randIndex = rand.Next(0, cardIndexs.Count);

        player.PutCard(DeckTag.PUT_DECK,
            cardIndexs.ElementAt(randIndex),
            true);

        return true;
    }
}
