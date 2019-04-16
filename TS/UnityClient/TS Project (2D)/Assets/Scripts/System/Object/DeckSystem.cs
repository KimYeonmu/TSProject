using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

public enum DeckTag
{
    DRAW_DECK,
    PUT_DECK,
    ANIMATION_RIGHT_DECK,
    ANIMATION_LEFT_DECK
}

public class DeckSystem : SingletonBase<DeckSystem>
{
    [HideInInspector] public List<Deck> Decks = new List<Deck>();

    void Awake()
    {
        CreateDeck(Vector2.up * 0);
        CreateDeck(Vector2.down * 3.5f);
        CreateDeck(Vector2.right * 10);
    }

    public void CreateDeck(Vector3 pos)
    {
        var obj = new GameObject("Deck");

        var deck = obj.AddComponent<Deck>();
        deck.SetParent(transform);
        deck.SetPosition(pos);
        deck.SetScale(Vector2.one);

        Decks.Add(deck);
    }

    public void AllMoveCardDecktoDeck(DeckTag deckTag1, DeckTag deckTag2, float delayTime, float intervalTime, float moveTime)
    {
        int tag1 = (int)deckTag1;
        int tag2 = (int)deckTag2;

        Observable.Timer(TimeSpan.FromSeconds(delayTime))
            .Subscribe(_ => Decks[tag1].AllMoveCardDecktoDeck(Decks[tag2], intervalTime, moveTime));
    }

    public void MoveCardDecktoDeck(DeckTag deckTag1, DeckTag deckTag2, float delayTime, int cardNum, float intervalTime, float moveTime)
    {
        int tag1 = (int)deckTag1;
        int tag2 = (int)deckTag2;

        Observable.Timer(TimeSpan.FromSeconds(delayTime))
            .Subscribe(_ => Decks[tag1].MoveCardDecktoDeck(Decks[tag2], cardNum, intervalTime, moveTime));
    }

    public void ShuffleDeck(DeckTag deckTag, int shuffleNum)
    {
        Decks[(int)deckTag].ShuffleDeck(shuffleNum);
    }


    /// <summary>덱 오브젝트 반환 함수</summary>
    /// <param name="deckTag">덱 tag</param>
    /// <returns>덱 오브젝트</returns>
    public Deck GetDeck(DeckTag deckTag)
    {
        return Decks[(int)deckTag];
    }

    /// <summary>덱의 맨 위에 있는 카드를 가져옴 </summary>
    /// <param name="deckTag">덱 tag</param>
    /// <returns>맨 위 카드</returns>
    public Card GetTopCardWithDeck(DeckTag deckTag)
    {
        return Decks[(int)deckTag].GetTopCard();
    }

    /// <summary>덱의 맨 위에 있는 카드를 큐에서 빼지 않고 반환 </summary>
    /// <param name="deckTag">덱 tag</param>
    /// <returns>맨 위 카드</returns>
    public Card GetTopCardPeekWithDeck(DeckTag deckTag)
    {
        return Decks[(int)deckTag].GetTopCardPeek();
    }

    /// <summary>덱이 가지고 있는 카드 수 반환 </summary>
    /// <param name="deckTag">덱 tag</param>
    /// <returns>카드 개수</returns>
    public int GetCardCountWithDeck(DeckTag deckTag)
    {
        return Decks[(int) deckTag].DeckList.Count;
    }
}

