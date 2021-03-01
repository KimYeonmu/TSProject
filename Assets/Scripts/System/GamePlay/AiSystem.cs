using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class AiSystem : SingletonBase<AiSystem>
{
    public BoolReactiveProperty IsStartAi = new BoolReactiveProperty(false);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupOneCardAi()
    {
        IsStartAi.Subscribe(value =>
        {
            if (value)
            {
                Debug.Log("ai start");
                var cardIndex = 0;
                var playerId = TurnSystem.GetInstance().PlayerNowTurn.Value;
                var isPut = PlayerSystem.GetInstance().GetPlayerIsPutCard(playerId);

                Observable.Interval(TimeSpan.FromSeconds(0.5f))
                    .TakeWhile(_ => RandomCard(playerId, ref cardIndex) >= 0)
                    .Subscribe(i =>
                        {
                            var card = PlayerSystem.GetInstance().GetPlayerCard(playerId, cardIndex);

                            PlayerSystem.GetInstance().PlayerPutCard(DeckTag.PUT_DECK, playerId, cardIndex, true);

                            RuleSystem.GetInstance().CheckSpecialCard(card, isPut);
                        },
                        () =>
                        {
                            IsStartAi.Value = false;
                            TurnSystem.GetInstance().EndTurn();
                        });
            }
        });
    }

    public int RandomCard(string playerId, ref int cardIndex)
    {
        var player = PlayerSystem.GetInstance().GetPlayer(playerId);

        var cardIndexs = new List<int>();

        var card = DeckSystem.GetInstance().GetTopCardPeekWithDeck(DeckTag.PUT_DECK);

        for (int i = 0; i < player.PlayerCard.Count; i++)
        {
            if (RuleSystem.GetInstance().CompareCard(
                card,
                player.PlayerCard[i],
                player.IsPutCard))
            {
                cardIndexs.Add(i);
            }
        }

        if (cardIndexs.Count <= 0)
        {
            Debug.Log("no card");
            return -1;
        }

        var rand = new Random();

        var randIndex = rand.Next(0, cardIndexs.Count - 1);

        cardIndex = cardIndexs[randIndex];

        return 1;
    }
}
