using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardText : IObject
{
    private TextData _text;

    public new void Awake()
    {
        base.Awake();

        _text = GetComponent<TextData>();

        gameObject.name = "Text";
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string str)
    {
        _text.Text.text = str;
    }

    public void SetColor(Color textColor)
    {
        _text.Text.color = textColor;
    }

    public void SetFontSize(int textSize)
    {
        _text.Text.fontSize = textSize;    
    }

    public void SetSortingOrder(int sortOrder)
    {
        _text.TextRenderer.sortingOrder = sortOrder;
    }

    public void SetFont(Font font)
    {
        _text.Text.font = font;
        _text.TextRenderer.material = font.material;
    }

    public string GetText()
    {
        return _text.Text.text;
    }
}
