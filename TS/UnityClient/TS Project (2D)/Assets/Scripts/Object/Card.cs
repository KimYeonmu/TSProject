using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : IObject
{
    public List<Shape> Shapes = new List<Shape>();
    public List<CardText> Texts = new List<CardText>();
    public Sprite BackCardSprite;

    public bool IsBack = false;
    public bool IsAttackCard;
    
    public int AttackCardNum;

    private CardData _data;

    public new void Awake()
    {
        base.Awake();

        gameObject.name = "Card";

        _data = GetComponent<CardData>();
        _data.CardRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start ()
    {
	}
    

    // Update is called once per frame
    void Update ()
    {
        
	}

    public void SetCardIndex(int index)
    {
        _data.CardIndex = (CardTag)index;
    }

    public void SetBackgroundSprite(Sprite sprite)
    {
        _data.CardRenderer.sprite = sprite;
    }

    public void SetBackCardSprite(Sprite sprite)
    {
        BackCardSprite = sprite;
    }

    public void SetSortingOrder(int sortOrder)
    {
        _data.CardRenderer.sortingOrder = (sortOrder * 10 + 1);

        for (int i = 0; i < Shapes.Count; i++)
        {
            Shapes[i].SetSortingOrder(sortOrder * 10 + 2);
        }

        for (int i = 0; i < Texts.Count; i++)
        {
            Texts[i].SetSortingOrder(sortOrder * 10 + 3);
        }
    }

    public void IsShapesActive()
    {
        for (int i = 0; i < Shapes.Count; i++)
        {
            Shapes[i].gameObject.SetActive( !Shapes[i].gameObject.activeSelf );
        }     
    }

    public void IsTextsActive()
    {
        for (int i = 0; i < Texts.Count; i++)
        {
            Texts[i].gameObject.SetActive(!Texts[i].gameObject.activeSelf);
        }
    }
    
    public CardTag GetCardIndex()
    {
        return _data.CardIndex;
    }

    public ShapeTag GetShapeIndex()
    {
        return Shapes[0].GetShapeIndex();
    }

    public int GetCardScore()
    {
        int shape = (int)GetShapeIndex();
        int cardNum = (int) GetCardIndex();

        return shape + cardNum;
    }
}
