using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : SingletonBase<GameManager>
{
    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        
        //NetworkSystem.GetInstance().StartConnect();

        CardSystem.GetInstance().AddAllCard();
        CardSystem.GetInstance().SetCardActive(true);

        //CardSystem.GetInstance().AllCardMoveDeck(DeckSystem.GetInstance().GetDeck(DeckTag.ANIMATION_RIGHT_DECK));
    }

    public IEnumerator StartGame()
    {
        StartCoroutine(PlayerSystem.GetInstance().ShareCard(6, 1));

        yield return new WaitForSeconds(PlayerSystem.GetInstance().Players.Count * 1);

        DeckSystem.GetInstance().MoveCardDecktoDeck(DeckTag.DRAW_DECK, DeckTag.PUT_DECK, 1, 0, 3);

        yield return new WaitForSeconds(1);

        PlayerSystem.GetInstance().PlayerCardReverse(PlayerSystem.GetInstance().MyPlayerIndex);

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(TurnSystem.GetInstance().StartTurn());
    }
}
