using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InputSystem : SingletonBase<InputSystem>
{
    public bool IsHoldCard;
    public int HoldCardNum;

    private List<Card> _myCardList;
    private List<Vector2> _myCardPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TouchUpdate()
    {
        if (PlayerSystem.GetInstance().Players.Count <= 0)
            return;

        Vector2 pos = Vector2.zero;
        var myPlayerIdx = PlayerSystem.GetInstance().GetMyPlayerIndex();

        if (myPlayerIdx < 0)
            return;

        //int myPlayerIndex = PlayerSystem.GetInstance().MyPlayerIndex;

        //for (int i = 0; i < Input.touchCount; i++)
        //{
        //    Touch touch = Input.GetTouch(i);

        //    pos = Camera.main.ScreenToWorldPoint(touch.position);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        _players[MyPlayerIndex].SelectCard_Began(pos);
        //        Debug.Log("begin");
        //    }

        //    else if (touch.phase == TouchPhase.Moved)
        //    {
        //        _players[MyPlayerIndex].SelectCard_Moved(pos);
        //        Debug.Log("moved");
        //    }

        //    else if (touch.phase == TouchPhase.Ended)
        //    {
        //        _players[MyPlayerIndex].SelectCard_Ended();
        //        Debug.Log("end");
        //    }
        //}

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            SelectCard_Began(pos, myPlayerIdx);
        }

        if (Input.GetMouseButton(0))
        {
            SelectCard_Moved(pos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectCard_Ended(myPlayerIdx);
        }
    }

    public void SelectCard_Began(Vector2 touchPos, int playerIdx)
    {
        BoxCollider2D collider = null;

        _myCardList = PlayerSystem.GetInstance().Players[playerIdx].PlayerCard;
        _myCardPos = PlayerSystem.GetInstance().Players[playerIdx].CardPositionList;

        for (int i = _myCardList.Count - 1; i >= 0; i--)
        {
            collider = _myCardList[i].gameObject.GetComponent<BoxCollider2D>();

            if (collider.OverlapPoint(touchPos))
            {
                _myCardList[i].SetScale(Vector2.one * 1.2f);
                _myCardList[i].SetPosition(_myCardPos[i]);
                break;
            }
        }
    }

    public void SelectCard_Moved(Vector2 touchPos)
    {
        BoxCollider2D collider2D;

        Vector2 vc = touchPos;

        if (IsHoldCard)
        {
            _myCardList[HoldCardNum].SetPosition(vc + Vector2.down * 1.3f);
            return;
        }

        for (int i = _myCardList.Count - 1; i >= 0; i--)
        {
            collider2D = _myCardList[i].gameObject.GetComponent<BoxCollider2D>();

            if (collider2D.OverlapPoint(touchPos))
            {
                _myCardList[i].SetScale(Vector2.one * 1.2f);
                _myCardList[i].SetPosition(_myCardPos[i] - Vector2.up);

                if (touchPos.y > -SceneSystem.GetInstance().ScreenWorldPoint.y / 1.3f)
                {
                    HoldCardNum = i;
                    IsHoldCard = true;

                    _myCardList[HoldCardNum].transform.DOScale(Vector3.one, 0.5f);

                    _myCardList[i].SetPosition(vc + Vector2.down * 1.5f);
                    _myCardList[i].SetSortingOrder(100);
                }

                for (int j = 0; j < _myCardList.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    _myCardList[j].SetScale(Vector2.one);
                    _myCardList[j].SetPosition(_myCardPos[j]);
                }

                break;
            }
        }
    }

    public void SelectCard_Ended(int playerIdx)
    {
        if (IsHoldCard == true)
        {
            IsHoldCard = false;

            var isPut = PlayerSystem.GetInstance().Players[playerIdx].IsPutCard;
            var isCompare = RuleSystem.GetInstance().CompareCard(
                DeckSystem.GetInstance().GetTopCardPeekWithDeck(DeckTag.PUT_DECK),
                _myCardList[HoldCardNum],
                isPut);

            if (_myCardList[HoldCardNum].GetPosition().y < -SceneSystem.GetInstance().ScreenSize.y * 0.4f ||
                !isCompare)
            {
                PlayerSystem.GetInstance().Players[playerIdx].RePosition();
                PlayerSystem.GetInstance().Players[playerIdx].ReScale();
            }
            else
            {
                _myCardList[HoldCardNum].SetSortingOrder(HoldCardNum + 1);

                RuleSystem.GetInstance().CheckSpecialCard(
                    _myCardList[HoldCardNum].GetCardIndex(),
                    isPut);

                PlayerSystem.GetInstance().PlayerPutCard(DeckTag.PUT_DECK, 
                    PlayerSystem.GetInstance().MyPlayerId,
                    HoldCardNum);
            }
        }
    }
}
