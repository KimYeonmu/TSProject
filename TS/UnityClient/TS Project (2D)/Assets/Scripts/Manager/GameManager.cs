using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : SingletonBase<GameManager>
{
    public bool IsStartGame = false;
    public Image WhiteImage;

    void Awake()
    {
        WhiteImage = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        //NetworkSystem.GetInstance().StartConnect();
    }

    public IEnumerator StartGame()
    {
        StartCoroutine(PlayerSystem.GetInstance().ShareCard(DeckTag.DRAW_DECK, 12, 0.1f));

        yield return new WaitForSeconds(PlayerSystem.GetInstance().Players.Count * 1);

        DeckSystem.GetInstance().MoveCardDecktoDeck(DeckTag.DRAW_DECK, DeckTag.PUT_DECK, 3, 1, 0, 0.5f);

        yield return new WaitForSeconds(1);

        PlayerSystem.GetInstance().PlayerCardReverse(PlayerSystem.GetInstance().GetMyPlayerIndex(), 0.5f);

        yield return new WaitForSeconds(1);

        IsStartGame = true;

        TurnSystem.GetInstance().StartTurn();
    }

    /// <summary>플레이어 카드가 룰의 Max 카드를 넘었을 때 </summary>
    /// <returns>플레이어가 패배했다면 true, 아니라면 false</returns>
    public bool CheckDefeatPlayer()
    {
        var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();
        var isPut = PlayerSystem.GetInstance().Players[nowTurn].IsPutCard;
        var damage = RuleSystem.GetInstance().GetAttackDamage(isPut);
        var playerCardCount = PlayerSystem.GetInstance().GetPlayerCardCount(nowTurn);
        var playerId = PlayerSystem.GetInstance().GetPlayerId(nowTurn);
        var isReverse = PlayerSystem.GetInstance().MyPlayerId == playerId ? true : false;

        if (damage + playerCardCount > RuleSystem.GetInstance().PlayerMaxCard)
        {
            PlayerSystem.GetInstance().PlayerCardMoveDeck(DeckTag.DRAW_DECK, nowTurn, isReverse, 0.1f);
            DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 1000);

            TurnSystem.GetInstance().NextTurn();
            PlayerSystem.GetInstance().RemovePlayer(playerId);
            TurnSystem.GetInstance().RemoveTurnPlayer(playerId);

            EndAttackTurn();
            return true;
        }

        return false;
    }

    /// <summary>플레이어 카드가 0장이 되었을 때 체크 </summary>
    /// <returns>0장이라면 true 아니라면 false</returns>
    public bool CheckWinPlayer()
    {
        var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();
        var count = PlayerSystem.GetInstance().GetPlayerCardCount(nowTurn);
        var id = PlayerSystem.GetInstance().GetPlayerId(nowTurn);

        if (count <= 0)
        {
            TurnSystem.GetInstance().NextTurn();
            PlayerSystem.GetInstance().RemovePlayer(id);
            TurnSystem.GetInstance().RemoveTurnPlayer(id);
            return true;
        }

        return false;
    }

    /// <summary>deck1의 카드가 부족할 때 deck2의 카드로 채우고 섞음 </summary>
    /// <param name="deck1"></param>
    /// <param name="deck2"></param>
    public void FillDecktoDeckofAttack(DeckTag deck1, DeckTag deck2)
    {
        var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();
        var isPut = PlayerSystem.GetInstance().Players[nowTurn].IsPutCard;
        var damage = RuleSystem.GetInstance().GetAttackDamage(isPut);

        var deck2Count = DeckSystem.GetInstance().GetCardCountWithDeck(deck2);

        Action addCardAction = () =>
        {
            DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 1000);
            NextTurn(nowTurn, damage);
        };


        if (damage > deck2Count)
        {
            var count = DeckSystem.GetInstance().GetCardCountWithDeck(deck1);

            if (count + deck2Count <= damage)
                return;

            Debug.Log("move put -> draw : " + count);

            DeckSystem.GetInstance().MoveCardDecktoDeck(
                deck1,
                deck2,
                0,
                0,
                count - 1,
                0,
                0.2f,
                addCardAction);
        }
        else
        {
            NextTurn(nowTurn, damage);
        }
    }

    public void FillDecktoDeckofDraw(DeckTag deck1, DeckTag deck2, int damage)
    {
        Action addCardAction = () =>
        {
            DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 1000);
        };

        var count = DeckSystem.GetInstance().GetCardCountWithDeck(deck1);

        if (damage > count)
        {
            Debug.Log("move put -> draw : " + count);

            DeckSystem.GetInstance().MoveCardDecktoDeck(
                deck1,
                deck2,
                0,
                0,
                count - 1,
                0,
                0.2f,
                addCardAction);
        }
    }

    public void NextTurn(int playerIdx, int damage)
    {
        if (damage != 0)
        {
            PlayerSystem.GetInstance().PlayerAddCardWithDeck(DeckTag.DRAW_DECK, playerIdx, damage);
            EndAttackTurn();
        }

        TurnSystem.GetInstance().NextTurn();

        PlayerSystem.GetInstance().Players[playerIdx].IsPutCard = false;
    }

    public void EndAttackTurn()
    {
        RuleSystem.GetInstance().IsAttackTurn = false;
        RuleSystem.GetInstance().SaveAttackDamage = 0;
        AlertSystem.GetInstance().ClearAlert();
    }


    #region Fade In Out Img Function

    public void FadeInWhiteImg(float duration)
    {
        WhiteImage.color = new Color(1, 1, 1, 0);
        WhiteImage.gameObject.SetActive(true);
        WhiteImage.DOColor(Color.white, duration);
    }

    public void FadeOutWhiteImg(float duration)
    {
        WhiteImage.color = Color.white;
        WhiteImage.gameObject.SetActive(true);
        WhiteImage.DOColor(new Color(1, 1, 1, 0), duration)
            .OnComplete(() => WhiteImage.gameObject.SetActive(false));
    }

    #endregion

}
