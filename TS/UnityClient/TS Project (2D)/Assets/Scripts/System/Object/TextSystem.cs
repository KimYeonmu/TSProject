using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSystem : SingletonBase<TextSystem>
{
    public Font TextFont;

    private CardText _textClone;

    public void Awake()
    {
        _textClone = ResourceManager.Load("Prefabs/Object/CardText").GetComponent<CardText>();
    }

    public void InitizlizeText(GameObject parentObject, string str)
    {
        CardText text = parentObject.AddComponent<CardText>();
        text.SetText(str);
    }

    public void AddCardText(Card card, Vector3 textPos, Vector3 textScale, int direction)
    {
        CardText text = Instantiate(_textClone);
        
        text.SetText(CardIndexToString(((int)card.GetCardIndex() / 10)));
        text.SetParent(card.gameObject.transform);
        text.SetScale(textScale);
        text.SetPosition(textPos * direction);
        text.SetFontSize(40);
        

        if (card.GetShapeIndex() == ShapeTag.Club ||
            card.GetShapeIndex() == ShapeTag.Spade)
        {
            text.SetColor(Color.black);
        }
        else
        {
            text.SetColor(Color.red);
        }

        if (direction > 0)
            text.SetReverse();

        text.SetFont(TextFont);
        card.Texts.Add(text);
    }

    public string CardIndexToString(int index)
    {
        string str = new string(index.ToString().ToCharArray());

        switch (index)
        {
            case 1:
                str = "A";
                break;
            case 11:
                str = "J";
                break;
            case 12:
                str = "Q";
                break;
            case 13:
                str = "K";
                break;
            case 14:
                str = "J\nO\nK\nE\nR";
                break;
            case 15:
                str = "J\nO\nK\nE\nR";
                break;
        }

        return str;
    }
}
