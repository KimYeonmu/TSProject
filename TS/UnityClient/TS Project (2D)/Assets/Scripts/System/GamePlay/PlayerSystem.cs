using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;


public class PlayerSystem : SingletonBase<PlayerSystem>
{
    [HideInInspector] public List<Player> Players = new List<Player>();

    public int MyPlayerIndex;
    public string MyPlayerId;

    public void Start()
    {

    }

    /// <summary>플레이어 추가 </summary>
    /// <param name="playerTag">플레이어 위치 tag</param>
    /// <param name="id">플레이어 id</param>
    /// <param name="isAi">ai 인지?</param>
    public void AddPlayer(PlayerTag playerTag, string id, bool isAi)
    {
        Player player = gameObject.AddComponent<Player>();
        player.PlayerPlace = playerTag;
        player.IsAi = isAi;
        player.PlayerId = id;
        Players.Add(player);
    }

    public void RemovePlayer(int playerIdx)
    {
        Players.RemoveAt(playerIdx);
    }

    /// <summary>해당 id를 가진 플레이어에게 카드 추가 </summary>
    /// <param name="deckTag">카드를 가져올 덱</param>
    /// <param name="playerName">플레이어 이름</param>
    /// <param name="cardNum">카드 갯수</param>
    public void PlayerAddCardWithDeck(DeckTag deckTag, string playerId, int cardNum)
    {
        Observable.Interval(TimeSpan.FromSeconds(0.05f))
            .Take(cardNum)
            .Subscribe(_ =>
            {
                var player = Players.Find(p =>p.PlayerId == playerId);

                var card = DeckSystem.GetInstance().GetTopCardWithDeck(deckTag);

                if (player == null)
                {
                    Debug.Log("Player not found : " + playerId);
                    return;
                }

                if (player.PlayerId == MyPlayerId)
                    CardAnimationSystem.GetInstance().ReverseAnimation(card, 0.5f);

                player.AddPlayerCard(card);
            });
    }

    /// <summary>해당 id를 가진 플레이어에게 카드 추가 </summary>
    /// <param name="deckTag">카드를 가져올 덱</param>
    /// <param name="playerName">플레이어 이름</param>
    /// <param name="cardNum">카드 갯수</param>
    public void PlayerAddCardWithDeck(DeckTag deckTag, int playerIdx, int cardNum)
    {
        Observable.Interval(TimeSpan.FromSeconds(0.05f))
            .Take(cardNum)
            .Subscribe(_ =>
            {
                var player = Players[playerIdx];

                if (player == null)
                {
                    Debug.Log("Player not found : " + Players[playerIdx].PlayerId);
                    return;
                }

                var card = DeckSystem.GetInstance().GetTopCardWithDeck(deckTag);

                if (player.PlayerId == MyPlayerId)
                    CardAnimationSystem.GetInstance().ReverseAnimation(card, 0.5f);

                player.AddPlayerCard(card);
            });
    }

    /// <summary>플레이어가 카드를 덱에 놓음 </summary>
    /// <param name="deckTag">카드를 놓을 덱</param>
    /// <param name="playerName">플레이어 id</param>
    /// <param name="cardListIndex">플레이어의 카드 index</param>
    /// <param name="reverseTime"></param>
    public void PlayerPutCard(DeckTag deckTag, string playerId, int cardListIndex, float reverseTime = 0.5f)
    {
        var player = Players.Find(p => p.PlayerId == playerId);

        AlertSystem.GetInstance().AddAlerts(player.PlayerCard[cardListIndex]);
        player.PutCard(deckTag,cardListIndex,false,reverseTime);
    }

    /// <summary>플레이어의 모든 카드를 뒤집음 </summary>
    /// <param name="playerIndex">플레이어의 index</param>
    /// <param name="reverseTime">뒤집는 애니메이션 지속시간</param>
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
            deckTag,
            isReverse,
            reverseTime);
    }

    public void AllPlayerCardMoveDeck(DeckTag deckTag, bool isReverse, float reverseTime)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].AllCardMoveDeck(
                deckTag,
                isReverse,
                reverseTime);
        }
    }

    public IEnumerator ShareCard(DeckTag deck, int cardNum, float shareDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(shareDelay);

        int tempTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < cardNum; j++)
            {
                Card card = DeckSystem.GetInstance().GetTopCardWithDeck(deck);

                if (tempTurn + i >= Players.Count)
                    tempTurn -= Players.Count;

                Players[tempTurn + i].AddPlayerCard(card);

                yield return delay;
            }
        }
    }


    public int GetPlayerCardCount(int playerIdx)
    {
        return Players[playerIdx].PlayerCard.Count;
    }
}