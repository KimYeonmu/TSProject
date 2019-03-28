using UnityEngine;

/// <summary>카드 종류들 </summary>
public enum CardTag
{
    A = 10,
    N2 = 20,
    N3 = 30,
    N4 = 40,
    N5 = 50,
    N6 = 60,
    N7 = 70,
    N8 = 80,
    N9 = 90,
    N10 = 100,
    J = 110,
    Q = 120,
    K = 130,
    Joker = 140,
    JokerR = 150,
    CardEnd
}

/// <summary>카드에 들어가는 데이터 클래스 </summary>
public class CardData : MonoBehaviour
{
    public CardTag CardIndex = CardTag.A;
    public SpriteRenderer CardRenderer;
}