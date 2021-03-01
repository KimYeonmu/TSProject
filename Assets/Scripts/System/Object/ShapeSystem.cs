using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShapeSystem : SingletonBase<ShapeSystem>
{
    [HideInInspector] public List<Vector3> ShapePos = new List<Vector3>();
    [HideInInspector] public List<Sprite> ShapeSprites = new List<Sprite>();
    [HideInInspector] public List<int[]> ShapeNumberPos = new List<int[]>();

    private Shape[] _shapes = new Shape[Enum.GetNames(typeof(ShapeTag)).Length];

    public float ShapePosition = 1;

    private void Awake()
    {
        AddShapePos();
        AddShapeNumberPos();
        AddShapeSprite();

        foreach (var shape in (ShapeTag[]) Enum.GetValues(typeof(ShapeTag)))
        {
            _shapes[(int)shape] = ResourceManager.GetInstance().Load("Prefabs/Object/CardShape").GetComponent<Shape>();
            _shapes[(int)shape].SetShapeIndex((int)shape);
        }
    }

    public void CreateCardShapeCenter(Card card, int shapeIndex)
    {
        int cardIndex = (int)card.GetCardIndex() / 10;
        int shapeSpriteIndex = GetShapeSpriteIndex((int)card.GetCardIndex(),shapeIndex);
        int posIndex = cardIndex > 10 ? 0 : cardIndex - 1;
        int nShapes = cardIndex > 10 ? 1 : cardIndex;

        for (var i = 0; i < nShapes; i++)
        {
            var shapeCenter = Instantiate(_shapes[shapeIndex]);
            shapeCenter.SetParent(card.gameObject.transform);
            shapeCenter.SetPosition(ShapePos[ShapeNumberPos[posIndex][i]] * ShapePosition);
            shapeCenter.SetShapeSprite(ShapeSprites[shapeSpriteIndex]);

            if (shapeCenter.GetPosition().y < 0)
                shapeCenter.SetReverse();

            card.Shapes.Add(shapeCenter);
        }
    }
    

    public void InitizlizeCardShapeSide(Card card, int shapeIndex)
    {
        Vector3 vScale = new Vector3(0.4f, 0.4f, 1);

        for (var i = 0; i < 2; i++)
        {
            var shapeSide = Instantiate(_shapes[shapeIndex]);
            shapeSide.SetParent(card.gameObject.transform);
            shapeSide.SetScale(vScale);
            shapeSide.SetPosition(ShapePos[(int)ShapePosTag.SideLeft + i] * ShapePosition);
            shapeSide.SetShapeSprite(ShapeSprites[shapeIndex]);

            if (shapeSide.GetPosition().y < 0)
                shapeSide.SetReverse();

            card.Shapes.Add(shapeSide);
        }
    }

    private void AddShapePos()
    {
        ShapePos.Add(new Vector3(0.0f, 0.0f, 0));       // Center
        ShapePos.Add(new Vector3(0.0f, 0.60f, 0));      // CenterUp1
        ShapePos.Add(new Vector3(0.0f, 0.80f, 0));      // CenterUp2
        ShapePos.Add(new Vector3(0.0f, 1.10f, 0));      // CenterUp3
        ShapePos.Add(new Vector3(0.0f, -0.60f, 0));     // CenterDown1
        ShapePos.Add(new Vector3(0.0f, -0.80f, 0));     // CenterDown2
        ShapePos.Add(new Vector3(0.0f, -1.10f, 0));     // CenterDown3
        ShapePos.Add(new Vector3(-0.5f, 0.10f, 0));     // Left
        ShapePos.Add(new Vector3(-0.5f, 0.40f, 0));     // LeftUp1
        ShapePos.Add(new Vector3(-0.5f, 1.10f, 0));     // LeftUp2
        ShapePos.Add(new Vector3(-0.5f, -0.40f, 0));    // LeftDown1
        ShapePos.Add(new Vector3(-0.5f, -1.10f, 0));    // LeftDown2
        ShapePos.Add(new Vector3(0.5f, 0.10f, 0));      // Right
        ShapePos.Add(new Vector3(0.5f, 0.40f, 0));      // RightUp1
        ShapePos.Add(new Vector3(0.5f, 1.10f, 0));      // RightUp2
        ShapePos.Add(new Vector3(0.5f, -0.40f, 0));     // RightDown1
        ShapePos.Add(new Vector3(0.5f, -1.10f, 0));     // RightDown2
        ShapePos.Add(new Vector3(-0.87f, 1.18f, 0));    // SideLeft
        ShapePos.Add(new Vector3(0.87f, -1.18f, 0));    // SideRight
    }

    private void AddShapeNumberPos()
    {
        ShapeNumberPos.Add(new int[1] {(int)ShapePosTag.Center});

        ShapeNumberPos.Add(new int[2] {(int)ShapePosTag.CenterUp3,
                                       (int)ShapePosTag.CenterDown3} );

        ShapeNumberPos.Add(new int[3] {(int)ShapePosTag.CenterUp3,
                                       (int)ShapePosTag.Center,
                                       (int)ShapePosTag.CenterDown3});

        ShapeNumberPos.Add(new int[4] {(int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown2});

        ShapeNumberPos.Add(new int[5] {(int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.Center,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown2});

        ShapeNumberPos.Add(new int[6] {(int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.Left,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown2,
                                       (int)ShapePosTag.Right});

        ShapeNumberPos.Add(new int[7] {(int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.Left,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown2,
                                       (int)ShapePosTag.Right,
                                       (int)ShapePosTag.CenterDown1});

        ShapeNumberPos.Add(new int[8] {(int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.Left,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown2,
                                       (int)ShapePosTag.Right,
                                       (int)ShapePosTag.CenterUp1,
                                       (int)ShapePosTag.CenterDown1});

        ShapeNumberPos.Add(new int[9] {(int)ShapePosTag.LeftUp1,
                                       (int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown1,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.RightUp1,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown1,
                                       (int)ShapePosTag.RightDown2,
                                       (int)ShapePosTag.Center,});

        ShapeNumberPos.Add(new int[10] {(int)ShapePosTag.LeftUp1,
                                       (int)ShapePosTag.LeftUp2,
                                       (int)ShapePosTag.LeftDown1,
                                       (int)ShapePosTag.LeftDown2,
                                       (int)ShapePosTag.RightUp1,
                                       (int)ShapePosTag.RightUp2,
                                       (int)ShapePosTag.RightDown1,
                                       (int)ShapePosTag.RightDown2,
                                       (int)ShapePosTag.CenterUp2,
                                       (int)ShapePosTag.CenterDown2});
    }

    private void AddShapeSprite()
    {
        var shape = Resources.LoadAll("Sprite/Shape/BaseShape", typeof(Sprite));

        for (int i = 0; i < shape.Length; i++)
            ShapeSprites.Add(shape[i] as Sprite);
    }

    private int GetShapeSpriteIndex(int cardIndex, int shapeIndex)
    {
        int spriteIndex = 0;

        switch ((ShapeTag)shapeIndex)
        {
            case ShapeTag.Spade:
                spriteIndex = 0;
                break;
            case ShapeTag.Heart:
                spriteIndex = 1;
                break;
            case ShapeTag.Diamond:
                spriteIndex = 2;
                break;
            case ShapeTag.Club:
                spriteIndex = 3;
                break;
        }

        switch (cardIndex)
        {
            case 10:
                if (shapeIndex == (int) ShapeTag.Spade)
                {
                    return 18;
                }
                break;
            case 110:
                spriteIndex = 4 + (shapeIndex * 3);
                break;
            case 120:
                spriteIndex = 5 + (shapeIndex * 3);
                break;
            case 130:
                spriteIndex = 6 + (shapeIndex * 3);
                break;
            case (int)CardTag.Joker:
                return 16;
            case (int)CardTag.JokerR:
                return 17;
        }

        return spriteIndex;
    }
}