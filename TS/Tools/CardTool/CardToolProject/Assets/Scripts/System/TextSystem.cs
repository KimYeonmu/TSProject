using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSystem : SingletonBase<TextSystem>
{
    public Font TextFont;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitizlizeText(GameObject parentObject, string str)
    {
        TextCard text = parentObject.AddComponent<TextCard>();
        text.SetText(str);
    }

    public void AddCardText(Card card, Vector3 textPos, Vector3 textScale, int direction)
    {
        TextCard text = card.Object.AddComponent<TextCard>();
        text.SetText(CardIndexToString(((int)card.GetCardIndex() / 10)));
        text.SetParent(card.Object.transform);
        text.SetScale(textScale);
        text.SetPosition(textPos * direction);
        text.SetFontSize(40);
        text.SetFont(TextFont);

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
