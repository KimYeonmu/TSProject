using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : IObject
{
    private ShapeData _data;

    private void Awake()
    {
        Object = new GameObject("Shape");
        _data = Object.AddComponent<ShapeData>();
        _data.ShapeRenderer = Object.AddComponent<SpriteRenderer>();
        _data.ShapeRenderer.sortingLayerName = "Card";
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetShapeIndex(int shapeIndex)
    {
        _data.ShapeIndex = (ShapeTag)shapeIndex;
    }

    public void SetShapePos(int shapePos)
    {
        _data.ShapePos = (ShapePosTag)shapePos;
    }

    public void SetShapeSprite(Sprite sprite)
    {
        _data.ShapeRenderer.sprite = sprite;
    }

    public void SetSortingOrder(int sortOrder)
    {
        _data.ShapeRenderer.sortingOrder = sortOrder;
    }

    public ShapePosTag GetShapePos()
    {
        return _data.ShapePos;
    }

    public ShapeTag GetShapeIndex()
    {
        return _data.ShapeIndex;
    }
}
