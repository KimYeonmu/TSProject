using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class AiSystem : SingletonBase<AiSystem>
{
    public BoolReactiveProperty IsStartAi = new BoolReactiveProperty(false);
    
    // Start is called before the first frame update
    void Start()
    {
        IsStartAi.Subscribe(value =>
        {
            if (value)
            {
                var playerIndex = TurnSystem.GetInstance().GetNowTurnPlayerIndex();
                var cardIndex = 0;

                Observable.Interval(TimeSpan.FromSeconds(0.5f))
                    .TakeWhile(_=> RandomCard(playerIndex, ref cardIndex) >= 0)
                    .Subscribe(i =>
                    {
                        var player = PlayerSystem.GetInstance().Players[playerIndex];
                        player.PutCard(DeckTag.PUT_DECK,cardIndex,true);
                    },
                    () =>
                    {
                        IsStartAi.Value = false;
                        TurnSystem.GetInstance().EndTurn();
                    });
            }

        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int RandomCard(int playerIndex, ref int cardIndex)
    {
        var player = PlayerSystem.GetInstance().Players[playerIndex];

        var cardIndexs = new List<int>();

        var card = DeckSystem.GetInstance().GetTopCardPeekWithDeck(DeckTag.PUT_DECK);

        Debug.Log("Deck Card : " + card.GetCardIndex() + " shape : " + card.GetShapeIndex());

        for (int i = 0; i < player.PlayerCard.Count; i++)
        {
            if (RuleSystem.GetInstance().CompareCard(
                card,
                player.PlayerCard[i],
                player.IsPutCard))
            {
                Debug.Log("compare d: " + card.GetCardIndex() + card.GetShapeIndex());
                Debug.Log("compare p: " + player.PlayerCard[i].GetCardIndex() + player.PlayerCard[i].GetShapeIndex());
                cardIndexs.Add(i);
            }
        }

        Debug.Log("Card Index : " + cardIndexs.Count);

        if (cardIndexs.Count <= 0)
            return -1;

        var rand = new Random();

        var randIndex = rand.Next(0, cardIndexs.Count - 1);

        Debug.Log("Put card : " + player.PlayerCard[randIndex].GetCardIndex() + " " + player.PlayerCard[randIndex].GetShapeIndex());
        cardIndex = cardIndexs[randIndex];

        return 1;
    }
}
