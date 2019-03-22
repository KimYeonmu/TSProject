using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public enum PlayerTag
{
    PLAYER_BOTTOM = 0,
    PLAYER_TOP,
    PLAYER_LEFT_DOWN,
    PLAYER_LEFT_UP,
    PLAYER_RIGHT_UP,
    PLAYER_RIGHT_DOWN
}

public class Player : MonoBehaviour
{
    public bool IsDie;
    public bool IsHoldCard;
    public bool IsPutCard = false;
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

    /// <summary>플레이어에 카드를 추가</summary>
    /// <param name="card"></param>
    public void AddPlayerCard(Card card)
    {
        PlayerCard.Add(card);

        card.SetSortingOrder(PlayerCard.Count);

        if (PlayerCard.Count > 20)
            IsDie = true;

        RePosition();
    }

    public void RePosition()
    {
        CardPositionList.Clear();

        if (PlayerCard.Count <= 0)
        {
            return;
        }

        Vector2 screen = SceneSystem.GetInstance().ScreenSize;

        Vector2 cardDistance = Vector2.zero;
        Vector2 startPoint = Vector2.zero;
        Vector2 minusDistance = Vector2.zero;
        Vector2 increateDistance = Vector2.zero;
        Vector3 angle = Vector3.zero; // 카드의 각도

        float increaseAngle = 0; // 카드가 추가될때 증가되는 각도

        //if (PlayerPlace == PlayerTag.PLAYER_BOTTOM)
        //    minusDistance.x = screen.x / 24 * PlayerCard.Count / 2 - 1.2f;
        //else
        //    minusDistance.y

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

            PlayerCard[i].transform.DOMove(screen, 0.5f);



            PlayerCard[i].transform.DORotate(Vector3.forward * (i - PlayerCard.Count / 2) * increaseAngle + angle, 0.1f);

            PlayerCard[i].SetSortingOrder(i);

            CardPositionList.Add(screen);
        }
    }

    public void ReScale()
    {
        for (int i = 0; i < PlayerCard.Count; i++)
        {
            PlayerCard[i].SetScale(Vector2.one);
        }
    }

    public void RevertPosition()
    {
        for (int i = 0; i < PlayerCard.Count; i++)
        {
            PlayerCard[i].SetPosition(CardPositionList[i]);
        }
    }

    public void AllCardMoveDeck(Deck cardDeck, bool isReverse, float reverseTime)
    {
        for (int i = 0; i < PlayerCard.Count; i++)
        {
            PutCard(cardDeck, i, isReverse, reverseTime);
        }
    }

    public void AllCardReverse(float time)
    {
        for (int i = 0; i < PlayerCard.Count; i++)
        {
            CardAnimationSystem.GetInstance().ReverseAnimation(PlayerCard[i], time);
        }
    }

    public void PutCard(Deck putDeck, int playerCardIndex, bool isBack, float reverseTime)
    {
        //BoxCollider2D temp = PlayerCard[playerCardIndex].gameObject.GetComponent<BoxCollider2D>();
        putDeck.AddCard(PlayerCard[playerCardIndex]);

        PlayerCard[playerCardIndex].transform.DOMove(Vector3.zero, 0.5f);
        PlayerCard[playerCardIndex].transform.DORotate(Vector3.zero, 0.5f);

        if (isBack != PlayerCard[playerCardIndex].IsBack)
        {
            CardAnimationSystem.GetInstance().ReverseAnimation(
                PlayerCard[playerCardIndex], reverseTime);
        }

        
        PlayerCard.RemoveAt(playerCardIndex);

        RePosition();
    }

    public void EndTurn()
    {
        if (IsPutCard == false)
        {
            NetworkSystem.GetInstance().SendServer(string.Format("ADD-CARD:{0}:{1}",PlayerId,1));
            return;
        }

        IsPutCard = false;
    }

    public void SelectCard_Began(Vector2 touchPos)
    {
        BoxCollider2D collider = null;

        for (int i = PlayerCard.Count - 1; i >= 0; i--)
        {
            collider = PlayerCard[i].gameObject.GetComponent<BoxCollider2D>();

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
            PlayerCard[HoldCardNum].SetPosition(vc + Vector2.down * 1.3f);
            return;
        }

        for (int i = PlayerCard.Count - 1; i >= 0; i--)
        {
            collider2D = PlayerCard[i].gameObject.GetComponent<BoxCollider2D>();
            
            if (collider2D.OverlapPoint(touchPos))
            {
                PlayerCard[i].SetScale(Vector2.one * 1.2f);
                PlayerCard[i].SetPosition(CardPositionList[i] + Vector2.up * 1);

                if (touchPos.y > -SceneSystem.GetInstance().ScreenWorldPoint.y / 1.3f)
                {
                    HoldCardNum = i;
                    IsHoldCard = true;

                    PlayerCard[HoldCardNum].transform.DOScale(Vector3.one, 2);

                    PlayerCard[i].SetPosition(vc + Vector2.down * 1.5f);
                    PlayerCard[i].SetSortingOrder(100);
                }

                for (int j = 0; j < PlayerCard.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    PlayerCard[j].SetScale(Vector2.one);
                    PlayerCard[j].SetPosition(CardPositionList[j]);
                }

                break;
            }
        }
    }

    public void SelectCard_Ended()
    {
        if (IsHoldCard == true)
        {
            IsHoldCard = false;

            var isCompare = DeckSystem.GetInstance().CompareCard(DeckTag.PUT_DECK,
                PlayerCard[HoldCardNum].GetShapeIndex(),
                PlayerCard[HoldCardNum].GetCardIndex(),
                IsPutCard);

            if (PlayerCard[HoldCardNum].GetPosition().y < -SceneSystem.GetInstance().ScreenSize.y * 0.4f ||
                !isCompare)
            {
                RePosition();
                ReScale();
                return;
            }

            IsPutCard = true;
            PlayerCard[HoldCardNum].SetSortingOrder(HoldCardNum + 1);
            NetworkSystem.GetInstance().SendServer(string.Format("PUT-CARD:{0}:{1}", PlayerId, HoldCardNum));
        }
    }
}