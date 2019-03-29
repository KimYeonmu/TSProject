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

    public void AddPlayer(PlayerTag playerTag, string id, bool isAi)
    {
        Player player = gameObject.AddComponent<Player>();
        player.PlayerPlace = playerTag;
        player.IsAi = isAi;
        player.PlayerId = id;
        Players.Add(player);

        TurnSystem.GetInstance().AddTurnPlayer(id);
    }

    public void PlayerAddCard(DeckTag deckTag, string playerName, int cardNum)
    {
        Observable.Interval(TimeSpan.FromSeconds(0.05f))
            .Take(cardNum)
            .Subscribe(_ =>
            {
                var player = Players.Find(p =>p.PlayerId == playerName);

                var card = DeckSystem.GetInstance().GetTopCardWithDeck(DeckTag.DRAW_DECK);

                if (player == null)
                {
                    Debug.Log("Player not found : " + playerName);
                    return;
                }

                if (player.PlayerId == MyPlayerId)
                    CardAnimationSystem.GetInstance().ReverseAnimation(card, 0.5f);

                player.AddPlayerCard(card);
            });
    }

    public void PlayerPutCard(DeckTag deckTag, string playerName, int cardListIndex, float reverseTime = 0.5f)
    {
        var player = Players.Find(p => p.PlayerId == playerName);

        AlertSystem.GetInstance().AddAlerts(player.PlayerCard[cardListIndex]);
        player.PutCard(deckTag,cardListIndex,false,reverseTime);
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

    public IEnumerator ShareCard(int cardNum, float shareDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(shareDelay);

        int tempTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < cardNum; j++)
            {
                Card card = DeckSystem.GetInstance().GetTopCardWithDeck(DeckTag.DRAW_DECK);

                if (tempTurn + i >= Players.Count)
                    tempTurn -= Players.Count;

                Players[tempTurn + i].AddPlayerCard(card);

                yield return delay;
            }
        }
    }

    public void CheckPutCardNowTurn(int playerIndex, bool IsAttack, int AttackDamage)
    {
        var player = Players[playerIndex];
        var id = Players[playerIndex].PlayerId;
        var damage = IsAttack ? AttackDamage : 1;

        Debug.Log(id);
        
        if (!player.IsPutCard)
        {
            PlayerAddCard(DeckTag.DRAW_DECK, id, damage);
            NetworkSystem.GetInstance().SendServer(string.Format("ADD-CARD:{0}:{1}", id, damage));
        }

        player.IsPutCard = false;
    }
}