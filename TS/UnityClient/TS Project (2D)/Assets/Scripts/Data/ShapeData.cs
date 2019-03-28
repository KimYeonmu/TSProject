using UnityEngine;

/// <summary>카드 모양 종류들 </summary>
public enum ShapeTag
{
    Spade = 0,
    Heart,
    Diamond,
    Club,
}

/// <summary>카드에 들어가는 모양의 위치 종류들 </summary>
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

/// <summary>카드에 들어가는 모양의 데이터 클래스 </summary>
public class ShapeData : MonoBehaviour
{
    public SpriteRenderer ShapeRenderer;
    public ShapeTag ShapeIndex = ShapeTag.Spade;

    public void Awake()
    {
        ShapeRenderer = GetComponent<SpriteRenderer>();
    }
}
