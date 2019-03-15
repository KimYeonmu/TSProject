using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class CardDeck : MonoBehaviour
{
    public GameCard CardTemplate;
    public GameObject Cards;

    private List<GameCard> _cardList = new List<GameCard>();

    public int CardCount
    {
        get { return _cardList.Count; }
    }

    protected void Start()
    {
        
    }

    public GameCard GetLastCard(bool remove)
    {
        if (!remove)
            return _cardList.Last();

        var card = _cardList.Last();
        _cardList.Remove(card);

        return card;
    }

    public List<GameCard> GetLastCards(int count, bool remove)
    {
        var cardList = new List<GameCard>();

        for (var i = 0; i < count; i++)
        {
            if (CardCount <= 0)
                break;

            cardList.Add(GetLastCard(remove));
        }

        return cardList;
    }
}