using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : IObject
{
    public List<Shape> Shapes = new List<Shape>();
    public List<TextCard> Texts = new List<TextCard>();
    public bool IsBack = false;
    public Sprite BackCardSprite;

    private CardData _data;
    private Vector2 _tempPos;

    private void Awake()
    {
        Object = new GameObject("Card");
        Object.layer = 8;
        Object.tag = "Card";
        _data = Object.AddComponent<CardData>();
        _data.CardRenderer = Object.AddComponent<SpriteRenderer>();
        _data.CardRenderer.sortingLayerName = "Card";
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
            Shapes[i].Object.SetActive( !Shapes[i].Object.activeSelf );
        }     
    }

    public void IsTextsActive()
    {
        for (int i = 0; i < Texts.Count; i++)
        {
            Texts[i].Object.SetActive(!Texts[i].Object.activeSelf);
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
