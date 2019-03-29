using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : SingletonBase<GameManager>
{
    public bool IsStartGame = false;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        //NetworkSystem.GetInstance().StartConnect();

        CardSystem.GetInstance().SetCardActive(true);
    }

    public IEnumerator StartGame()
    {
        StartCoroutine(PlayerSystem.GetInstance().ShareCard(6, 0.1f));

        yield return new WaitForSeconds(PlayerSystem.GetInstance().Players.Count * 1);

        DeckSystem.GetInstance().MoveCardDecktoDeck(DeckTag.DRAW_DECK, DeckTag.PUT_DECK,3, 1, 0, 0.5f);

        yield return new WaitForSeconds(1);

        PlayerSystem.GetInstance().PlayerCardReverse(PlayerSystem.GetInstance().MyPlayerIndex, 0.5f);

        yield return new WaitForSeconds(1);

        IsStartGame = true;

        TurnSystem.GetInstance().StartTurn();
    }
}
