using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSystem : SingletonBase<CardSystem>
{
    public int NPlayer = 2;

    public Vector3 CardScale;
    public Vector2 DeckDistance = new Vector2(0.005f,0.005f);

    public Sprite BackCardSprite;
    public Sprite BackGroundSprite;

    public List<Card> Cards = new List<Card>();

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
        Card card = gameObject.AddComponent<Card>();
        card.SetCardIndex((cardIndex + 1) * 10);
        card.SetBackgroundSprite(BackGroundSprite);
        card.SetBackCardSprite(BackCardSprite);

        if (cardIndex < 10)
            ShapeSystem.GetInstance().InitizlizeCardShapeCenter(card, shapeIndex, cardIndex + 1);
        else
            ShapeSystem.GetInstance().InitizlizeCardShapeCenter(card, shapeIndex, 1);

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

        card.SetScale(CardScale);
        card.SetSortingOrder(Cards.Count);

        Cards.Add(card);
    }

    public void AddCardInDeck(Card card)
    {
        card.SetSortingOrder(Cards.Count);
        Cards.Add(card);
    }


    public IEnumerator SinAnimation()
    {
        WaitForSeconds delay = new WaitForSeconds(5);

        yield return delay;

        delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < Cards.Count; i++)
        {
            StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                Cards[i].GetPosition,
                Cards[i].SetPosition,
                new Vector2(
                    Mathf.Sin((360 / Cards.Count + 1) * i * Mathf.Deg2Rad) * 5,
                    Mathf.Cos((360 / Cards.Count + 1) * i * Mathf.Deg2Rad) * 5
                    ),
                5));

            yield return delay;
        }

        delay = new WaitForSeconds(2);
        yield return delay;

        yield return StartCoroutine(AllCardReverse());
    }

    public IEnumerator AllCardReverse()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            StartCoroutine(CardAnimationSystem.GetInstance().ReverseAnimation(Cards[i],3));
        }

        yield return null;
    }

    public Sprite GetCardSprite(bool isBack)
    {
        if (isBack == true)
            return BackCardSprite;

        return BackGroundSprite;
    }
}
