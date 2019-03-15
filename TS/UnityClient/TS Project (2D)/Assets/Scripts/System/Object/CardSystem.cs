using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSystem : SingletonBase<CardSystem>
{
    public Vector3 CardScale;
    public Vector2 DeckDistance = new Vector2(0.005f,0.005f);

    public Sprite BackCardSprite;
    public Sprite BackGroundSprite;

    [HideInInspector] public List<Card> Cards = new List<Card>();

    private Vector3 NormalTextPos = new Vector3(2.9f, -4.7f, 0) * 0.3f;
    private Vector3 NormalTextScale = new Vector3(0.2f, 0.2f, 1) * 0.3f;

    private Vector3 JokerTextPos = new Vector3(3, -3, 0) * 0.3f;
    private Vector3 JokerTextScale = new Vector3(0.18f, 0.18f, 1) * 0.3f;

    void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void AddAllCard()
    {
        // 1 ~ 10, JQK Card Add

        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 13; j++)
            {
                this.CreateCard(i, j);
            }
        }
        
        this.CreateCard(0, 13);
        this.CreateCard(1, 14);
    }

    public void CreateCard(int shapeIndex, int cardIndex)
    {
        var obj = new GameObject("Card");
        
        Card card = obj.AddComponent<Card>();
        card.SetCardIndex((cardIndex + 1) * 10);
        card.SetBackgroundSprite(BackGroundSprite);
        card.SetBackCardSprite(BackCardSprite);
        card.SetParent(transform);
        
        ShapeSystem.GetInstance().CreateCardShapeCenter(card, shapeIndex);

        if (cardIndex < 13)
        {
            ShapeSystem.GetInstance().InitizlizeCardShapeSide(card, shapeIndex);

            TextSystem.GetInstance().AddCardText(card, NormalTextPos, NormalTextScale, 1);
            TextSystem.GetInstance().AddCardText(card, NormalTextPos, NormalTextScale, -1);
        }
        else
        {
            TextSystem.GetInstance().AddCardText(card, JokerTextPos, JokerTextScale, 1);
            TextSystem.GetInstance().AddCardText(card, JokerTextPos, JokerTextScale, -1);
        }

        card.gameObject.AddComponent<BoxCollider2D>();
        card.SetScale(CardScale);
        card.SetSortingOrder(Cards.Count);
        card.gameObject.SetActive(false);
        Cards.Add(card);
    }

    public void AddCardInDeck(Card card)
    {
        card.SetSortingOrder(Cards.Count);
        Cards.Add(card);
    }


    public IEnumerator AllCardReverse()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            StartCoroutine(CardAnimationSystem.GetInstance().ReverseAnimation(Cards[i],3));
        }

        yield return null;
    }

    public void AllCardMoveDeck(Deck deck)
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            deck.AddCard(Cards[i]);
            Cards[i].SetPosition(deck.GetPosition());
            Cards[i].SetScale(Vector2.one);
            Cards[i].SetParent(deck.gameObject.transform);
        }
    }

    public void SetCardActive(bool isActive)
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].gameObject.SetActive(isActive);
        }
    }

    public Sprite GetCardSprite(bool isBack)
    {
        if (isBack == true)
            return BackCardSprite;

        return BackGroundSprite;
    }
}
