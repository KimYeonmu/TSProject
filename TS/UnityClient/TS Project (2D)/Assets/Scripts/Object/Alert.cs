using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Alert : IObject
{
    public SpriteRenderer AlertRenderer;

    public CardTag AlertCardTag;
    public ShapeTag AlertShapeTag;

    public new void Awake()
    {
        base.Awake();
        gameObject.name = "Alert";

        AlertRenderer = gameObject.AddComponent<SpriteRenderer>();
        AlertRenderer.sortingLayerName = "Alert";
    }

    public void Start()
    {
    
    }

    public void SetSprite(Sprite sprite)
    {
        AlertRenderer.sprite = sprite;
    }
}

