using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PlayerSystem : SingletonBase<PlayerSystem>
{
    [HideInInspector] public List<Player> Players = new List<Player>();

    public int MyPlayerIndex;
    public string MyPlayerId;

    public void Start()
    {

    }

    public void AddPlayer(PlayerTag playerTag, string id, bool isAi)
    {
        Player player = gameObject.AddComponent<Player>();
        player.PlayerPlace = playerTag;
        player.IsAi = isAi;
        player.PlayerId = id;
        Players.Add(player);

        TurnSystem.GetInstance().AddTurnPlayer(id);
    }

    public IEnumerator PlayerAddCard(DeckTag deckTag, string playerName, int cardNum)
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < cardNum; j++)
            {
                if (Players[i].PlayerId.Equals(playerName))
                {
                    if (i == MyPlayerIndex)
                    {
                        CardAnimationSystem.GetInstance().ReverseAnimation(
                                DeckSystem.GetInstance().GetTopCardPeekWithDeck(DeckTag.DRAW_DECK),
                                3);
                    }

                    Players[i].AddPlayerCard(DeckSystem.GetInstance().GetTopCardWithDeck(deckTag));

                    yield return delay;
                }
            }
        }
    }

    public void PlayerPutCard(DeckTag deckTag, string playerName, int cardListIndex, float reverseTime)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].PlayerId.Equals(playerName))
            {
                AlertSystem.GetInstance().AddAlerts(Players[i].PlayerCard[cardListIndex]);

                Players[i].PutCard(
                    DeckSystem.GetInstance().GetDeck(deckTag),
                    cardListIndex,
                    false,
                    reverseTime);
            }
        }
    }

    public void PlayerCardReverse(int playerIndex, float reverseTime)
    {
        Players[playerIndex].AllCardReverse(reverseTime);
    }

    public void AllPlayerCardReverse(float reverseTime)
    {
        for (int i = 0; i < Players.Count; i++)
            Players[i].AllCardReverse(reverseTime);
    }

    public void PlayerCardMoveDeck(DeckTag deckTag, int playerIndex, bool isReverse, float reverseTime)
    {
        Players[playerIndex].AllCardMoveDeck(
            DeckSystem.GetInstance().GetDeck(deckTag),
            isReverse,
            reverseTime);
    }

    public void AllPlayerCardMoveDeck(DeckTag deckTag, bool isReverse, float reverseTime)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].AllCardMoveDeck(
                DeckSystem.GetInstance().GetDeck(deckTag),
                isReverse,
                reverseTime);
        }
    }

    public IEnumerator ShareCard(int cardNum, float shareDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(shareDelay);

        int tempTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < cardNum; j++)
            {
                Card card = DeckSystem.GetInstance().GetDeck(DeckTag.DRAW_DECK).GetTopCard();

                if (tempTurn + i >= Players.Count)
                    tempTurn -= Players.Count;

                Players[tempTurn + i].AddPlayerCard(card);

                yield return delay;
            }
        }
    }
}