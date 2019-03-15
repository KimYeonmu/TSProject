using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        CreateDeck(Vector2.up * 1.5f);
        CreateDeck(Vector2.down * 0.4f);
        CreateDeck(Vector2.right * 10);
    }

    public void CreateDeck(Vector3 pos)
    {
        Deck deck = gameObject.AddComponent<Deck>();
        deck.SetParent(transform);
        deck.SetPosition(pos);
        deck.SetScale(Vector2.one);

        Decks.Add(deck);
    }

    public Deck GetDeck(DeckTag deckTag)
    {
        return Decks[(int)deckTag];
    }

    public Card GetTopCardWithDeck(DeckTag deckTag)
    {
        return Decks[(int) deckTag].GetTopCard();
    }

    public Card GetTopCardPeekWithDeck(DeckTag deckTag)
    {
        return Decks[(int) deckTag].GetTopCardPeek();
    }

    public void AllMoveCardDecktoDeck(DeckTag deckTag1, DeckTag deckTag2, float intervalTime, float moveTime)
    {
        int tag1 = (int)deckTag1;
        int tag2 = (int)deckTag2;

        Decks[tag1].AllMoveCardDecktoDeck(Decks[tag2], intervalTime, moveTime);
    }

    public void MoveCardDecktoDeck(DeckTag deckTag1, DeckTag deckTag2, int cardNum, float intervalTime, float moveTime)
    {
        int tag1 = (int)deckTag1;
        int tag2 = (int)deckTag2;

        Decks[tag1].MoveCardDecktoDeck(Decks[tag2], cardNum, intervalTime, moveTime);
    }

    public void ShuffleDeck(DeckTag deckTag, int shuffleNum)
    {
        Decks[(int)deckTag].ShuffleDeck(shuffleNum);
    }

    public bool CompareCard(DeckTag deckTag, ShapeTag shapeTag, CardTag cardTag, bool IsPutCard)
    {
        ShapeTag deckShape = Decks[(int) deckTag].GetTopCardPeek().GetShapeIndex();
        CardTag deckCard = Decks[(int) deckTag].GetTopCardPeek().GetCardIndex();

        if (IsPutCard == true)
        {
            if (deckCard == cardTag ||
                deckCard == CardTag.Joker && cardTag == CardTag.JokerR ||
                deckCard == CardTag.JokerR && cardTag == CardTag.Joker)
            {
                return true;
            }

            return false;
        }

        if (deckCard == CardTag.Joker)
        {
            if (shapeTag == ShapeTag.Club || shapeTag == ShapeTag.Spade)
            {
                return true;
            }
        }
        
        else if (deckCard == CardTag.JokerR)
        {
            if (shapeTag == ShapeTag.Heart || shapeTag == ShapeTag.Diamond)
            {
                return true;
            }
        }

        if (cardTag == CardTag.Joker)
        {
            if (deckShape == ShapeTag.Club || deckShape == ShapeTag.Spade)
            {
                return true;
            }
        }

        else if (cardTag == CardTag.JokerR)
        {
            if (deckShape == ShapeTag.Heart || deckShape == ShapeTag.Diamond)
            {
                return true;
            }
        }

        if (deckShape == shapeTag || deckCard == cardTag)
            return true;

        return false;
    }

}

