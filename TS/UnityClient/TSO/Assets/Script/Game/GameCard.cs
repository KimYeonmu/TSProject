using GameCommon.Data;
using UnityEngine;

class GameCard : MonoBehaviour
{
    public GameObject Background;
    public GameObject Shape;

    private Card _cardData;
    public Card CardData
    {
        get { return _cardData; }
    }

    public void Initialize(Card card)
    {
        _cardData = card;
    }

    public void Initialize(CardShape shape, CardNumber number)
    {
        _cardData.Shape = shape;
        _cardData.Number = number;
    }

    private void SetSprite()
    {
    //    Background.GetComponent<SpriteRenderer>().sprite.
    }
}