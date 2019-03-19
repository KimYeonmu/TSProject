using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : IObject
{
    private ShapeData _data;

    public new void Awake()
    {
        base.Awake();

        gameObject.name = "Shape";
        _data = GetComponent<ShapeData>();
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

    public void SetShapeSprite(Sprite sprite)
    {
        _data.ShapeRenderer.sprite = sprite;
    }

    public void SetSortingOrder(int sortOrder)
    {
        _data.ShapeRenderer.sortingOrder = sortOrder;
    }

    public ShapeTag GetShapeIndex()
    {
        return _data.ShapeIndex;
    }
}
