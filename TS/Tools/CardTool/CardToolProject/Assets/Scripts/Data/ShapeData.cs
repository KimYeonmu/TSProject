using System.Collections.Generic;
using UnityEngine;

public enum ShapeTag
{
    Spade = 0,
    Heart,
    Diamond,
    Club,
    ShapeEnd
}

public enum ShapePosTag
{
    Center,
    CenterUp1,
    CenterUp2,
    CenterUp3,
    CenterDown1,
    CenterDown2,
    CenterDown3,
    Left,
    LeftUp1,
    LeftUp2,
    LeftDown1,
    LeftDown2,
    Right,
    RightUp1,
    RightUp2,
    RightDown1,
    RightDown2,
    SideLeft,
    SideRight,
}

public class ShapeData : MonoBehaviour
{
    public ShapePosTag ShapePos = ShapePosTag.Center;
    public SpriteRenderer ShapeRenderer;
    public ShapeTag ShapeIndex = ShapeTag.Spade;
}
