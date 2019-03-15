using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCard : IObject
{
    private TextData _data;

    private void Awake()
    {
        Object = new GameObject("Text");
        _data = Object.AddComponent<TextData>();
        _data.TextRenderer = Object.AddComponent<MeshRenderer>();
        _data.TextRenderer.sortingLayerName = "Card";
        
        _data.Text = Object.AddComponent<TextMesh>();
        _data.Text.anchor = TextAnchor.MiddleCenter;
        _data.Text.alignment = TextAlignment.Center;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string str)
    {
        _data.Text.text = str;
    }

    public void SetColor(Color textColor)
    {
        _data.Text.color = textColor;
    }

    public void SetFontSize(int textSize)
    {
        _data.Text.fontSize = textSize;    
    }

    public void SetSortingOrder(int sortOrder)
    {
        _data.TextRenderer.sortingOrder = sortOrder;
    }

    public void SetFont(Font font)
    {
        _data.Text.font = font;
    }

    public string GetText()
    {
        return _data.Text.text;
    }
}
