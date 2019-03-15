using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum PlayerTag
{
    PLAYER_BOTTOM = 0,
    PLAYER_LEFT_DOWN,
    PLAYER_LEFT_UP,
    PLAYER_TOP,
    PLAYER_RIGHT_UP,
    PLAYER_RIGHT_DOWN
}

public class Player : MonoBehaviour
{
    public bool IsTurn;
    public bool IsDie;
    public bool IsHoldCard;
    public bool IsPutCard;
    public bool IsAi;

    public int HoldCardNum;

    public PlayerTag PlayerPlace;
    public List<Card> PlayerCard = new List<Card>();
    public List<Vector2> CardPositionList = new List<Vector2>();
    public string PlayerId;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddPlayerCard(Card card)
    {
        PlayerCard.Add(card);

        card.SetSortingOrder(PlayerCard.Count);

        if (PlayerCard.Count > 20)
        {
            IsDie = true;
        }
    }

    public void RePosition()
    {
        CardPositionList.Clear();

        Vector2 screen = SceneSystem.GetInstance().ScreenSize;

        Vector2 cardDistance = Vector2.zero;
        Vector2 startPoint = Vector2.zero;
        Vector2 minusDistance = Vector2.zero;
        Vector2 increateDistance = Vector2.zero;
        Vector3 angle = Vector3.zero;                 // 카드의 각도

        float increaseAngle = 0;    // 카드가 추가될때 증가되는 각도

        switch (PlayerPlace)
        {
            case PlayerTag.PLAYER_BOTTOM:
                {
                    cardDistance.x = 0.44f;
                    minusDistance.x = screen.x / 24 * PlayerCard.Count / 2 - 1.2f;
                    startPoint = new Vector2(-0.84f, -8);
                }
                break;

            case PlayerTag.PLAYER_TOP:
                {
                    cardDistance.x = 0.19f;
                    minusDistance.x = screen.x / 20 * PlayerCard.Count / 6 - 1.2f;
                    increateDistance.y = PlayerCard.Count * 0.01f;
                    increaseAngle = 1;
                    startPoint = new Vector2(-1, 10);
                }
                break;

            case PlayerTag.PLAYER_LEFT_DOWN:
            case PlayerTag.PLAYER_LEFT_UP:
                {
                    cardDistance.y = 0.19f;
                    minusDistance.y = screen.x / 20 * PlayerCard.Count / 6 - 1.2f;
                    increateDistance.x = -PlayerCard.Count * 0.01f;
                    angle.z = 90;
                    increaseAngle = 1;

                    if (PlayerPlace == PlayerTag.PLAYER_LEFT_DOWN)
                        startPoint = new Vector2(-5, -3);
                    else if (PlayerPlace == PlayerTag.PLAYER_LEFT_UP)
                        startPoint = new Vector2(-5, 3.5f);
                }
                break;

            case PlayerTag.PLAYER_RIGHT_DOWN:
            case PlayerTag.PLAYER_RIGHT_UP:
                {
                    cardDistance.y = 0.19f;
                    minusDistance.y = screen.x / 20 * PlayerCard.Count / 6 - 1.2f;
                    increateDistance.x = PlayerCard.Count * 0.01f;
                    angle.z = -90;
                    increaseAngle = -1;

                    if (PlayerPlace == PlayerTag.PLAYER_RIGHT_DOWN)
                        startPoint = new Vector2(5, -3);
                    else if (PlayerPlace == PlayerTag.PLAYER_RIGHT_UP)
                        startPoint = new Vector2(5, 3.5f);
                }
                break;
        }

        startPoint -= new Vector2(PlayerCard[0].GetParent().localPosition.x,
                              PlayerCard[0].GetParent().localPosition.y);

        for (int i = 0; i < PlayerCard.Count; i++)
        {
            screen = startPoint + cardDistance * i - minusDistance;
            screen += -Mathf.Sin(180 / PlayerCard.Count * i * Mathf.Deg2Rad) * increateDistance;

            StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                PlayerCard[i].GetPosition,
                PlayerCard[i].SetPosition,
                screen,
                5));

            StartCoroutine(AnimationSystem.GetInstance().LerpRotateAnimation(
                PlayerCard[i].GetRotation,
                PlayerCard[i].SetRotation,
                Vector3.forward * (i - PlayerCard.Count / 2) * increaseAngle + angle,
                5));

            CardPositionList.Add(screen);
        }
    }

    public void AllCardMoveDeck(Deck cardDeck)
    {
        for (int i = 0; i < PlayerCard.Count; i++)
        {
            PutCard(cardDeck,i);
        }
    }

    public void AllCardReverse()
    {
        for (int i = 0; i < PlayerCard.Count; i++)
        {
            StartCoroutine(CardAnimationSystem.GetInstance().ReverseAnimation(PlayerCard[i], 3));
        }
    }

    public void PutCard(Deck putDeck, int playerCardIndex)
    {
        IsPutCard = true;

        StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
            PlayerCard[playerCardIndex].GetPosition,
            PlayerCard[playerCardIndex].SetPosition,
            putDeck.GetPosition(),
            5));

        StartCoroutine(AnimationSystem.GetInstance().LerpRotateAnimation(
            PlayerCard[playerCardIndex].GetRotation,
            PlayerCard[playerCardIndex].SetRotation,
            putDeck.GetRotation(),
            5));

        if (PlayerCard[playerCardIndex].IsBack == true)
        {
            StartCoroutine(CardAnimationSystem.GetInstance().ReverseAnimation(
                PlayerCard[playerCardIndex], 5));
        }

        putDeck.AddCard(PlayerCard[playerCardIndex]);
        PlayerCard.RemoveAt(playerCardIndex);
    }

    public void EndTurn()
    {
        IsTurn = false;
        IsPutCard = false;
    }

    public void SelectCard_Began(Vector2 touchPos)
    {
        BoxCollider2D collider = null;

        for (int i = PlayerCard.Count - 1; i >= 0; i--)
        {
            collider = PlayerCard[i].Object.GetComponent<BoxCollider2D>();

            if (collider.OverlapPoint(touchPos))
            {
                PlayerCard[i].SetScale(Vector2.one * 1.2f);
                PlayerCard[i].SetPosition(CardPositionList[i] + Vector2.up * 1);
                break;
            }
        }
    }

    public void SelectCard_Moved(Vector2 touchPos)
    {
        BoxCollider2D collider2D;

        Vector2 vc;

        vc.x = touchPos.x * SceneSystem.GetInstance().ScreenSize.x / SceneSystem.GetInstance().ScreenPoint.x;
        vc.y = touchPos.y * SceneSystem.GetInstance().ScreenSize.y / SceneSystem.GetInstance().ScreenPoint.y;

        if (IsHoldCard == true)
        {
            PlayerCard[HoldCardNum].SetPosition(vc + Vector2.down * 1.5f);

            return;
        }

        for (int i = PlayerCard.Count - 1; i >= 0; i--)
        {
            collider2D = PlayerCard[i].Object.GetComponent<BoxCollider2D>();

            if (collider2D.OverlapPoint(touchPos))
            {
                if (touchPos.y > -SceneSystem.GetInstance().ScreenWorldPoint.y / 1.4f)
                {
                    HoldCardNum = i;
                    IsHoldCard = true;

                    StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                        PlayerCard[HoldCardNum].GetScale,
                        PlayerCard[HoldCardNum].SetScale,
                        Vector2.one,
                        10));

                    PlayerCard[i].SetPosition(vc + Vector2.down * 1.5f);
                    PlayerCard[HoldCardNum].SetSortingOrder(100);
                    break;
                }
            }
            else
            {
                PlayerCard[i].SetScale(Vector2.one);
                PlayerCard[i].SetPosition(CardPositionList[i]);
            }
        }
    }

    public void SelectCard_Ended()
    {

        for (int i = 0; i < PlayerCard.Count; i++)
        {
            PlayerCard[i].SetScale(Vector2.one);

            StartCoroutine(AnimationSystem.GetInstance().LerpAnimation(
                PlayerCard[i].GetPosition,
                PlayerCard[i].SetPosition,
                CardPositionList[i],
                10));
        }

        if (IsHoldCard == true)
        {
            PlayerCard[HoldCardNum].SetSortingOrder(HoldCardNum + 1);
            IsHoldCard = false;
        }
    }
}