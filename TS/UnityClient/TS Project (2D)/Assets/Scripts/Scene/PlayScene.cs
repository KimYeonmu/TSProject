using System;
using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;


public class PlayScene : IScene
{
    public GameObject TurnEndBtnObject;
    public GameObject OneCardBtnObject;
    public GameObject ChatBtnObject;
    public GameObject ChatImgObject;

    public bool IsChatBtnClick = false;

    private bool _isShowBtn = false;

    public override void Awake()
    {


        base.Awake();

        //Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        //Screen.SetResolution(720, 1280, false);
        //SceneSystem.GetInstance().SetScenePos(new Vector2(0, -20));
        //SceneSystem.GetInstance().SetSceneScale(Vector2.zero);

        var size = GameObject.FindWithTag("Background").GetComponent<SpriteRenderer>().sprite.bounds.size;

        SceneSystem.GetInstance().SetScreenSize(new Vector2(size.x, size.y));
    }

    void Start()
    {
        GameManager.GetInstance().FadeOutWhiteImg(0.5f);

        CardSystem.GetInstance().AddAllCard();
        CardSystem.GetInstance().SetCardActive(true);

        CardSystem.GetInstance().AllCardMoveDeck(DeckSystem.GetInstance().GetDeck(DeckTag.ANIMATION_RIGHT_DECK));

        if (PlayerSystem.GetInstance().Players.Count <= 0)
        {
            PlayerSystem.GetInstance().MyPlayerId = "1";
            PlayerSystem.GetInstance().AddPlayer(PlayerTag.PLAYER_BOTTOM, "1", false);
            PlayerSystem.GetInstance().AddPlayer(PlayerTag.PLAYER_LEFT_DOWN, "3", true);
            PlayerSystem.GetInstance().AddPlayer(PlayerTag.PLAYER_TOP, "2", true);
            PlayerSystem.GetInstance().AddPlayer(PlayerTag.PLAYER_RIGHT_UP, "4", true);


            TurnSystem.GetInstance().AddTurnPlayer("1");
            TurnSystem.GetInstance().AddTurnPlayer("2");
            TurnSystem.GetInstance().AddTurnPlayer("3");
            TurnSystem.GetInstance().AddTurnPlayer("4");

            TurnSystem.GetInstance().SetFirstTurn("1");
            //NetworkSystem.GetInstance().SendServer("FIND-ROOM:" + "1");
        }

        StartCoroutine(StartScene());

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!GameManager.GetInstance().IsStartGame)
                    return;

                var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

                if (nowTurn < 0)
                    return;

                if (nowTurn == PlayerSystem.GetInstance().MyPlayerIndex)
                    TouchUpdate();
            });

        TurnSystem.GetInstance().PlayerNowTurn.Subscribe(name =>
        {
            //if (!GameManager.GetInstance().IsStartGame)
            //    return;

            Debug.Log("Now turn : " + name);
            var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

            if (nowTurn != PlayerSystem.GetInstance().MyPlayerIndex)
                AiSystem.GetInstance().IsStartAi.Value = true;

        });

        TurnSystem.GetInstance().IsFinishTurn.Subscribe(finish =>
        {
            if (!GameManager.GetInstance().IsStartGame)
                return;

            if (finish)
            {
                var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

                Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
                    {
                        var isPut = PlayerSystem.GetInstance().Players[nowTurn].IsPutCard;
                        var damage = RuleSystem.GetInstance().GetAttackDamage(isPut);
                        var playerCard = PlayerSystem.GetInstance().GetPlayerCardCount(nowTurn);

                        if (damage + playerCard > RuleSystem.GetInstance().PlayerMaxCard)
                        {
                            var isMyTurn = PlayerSystem.GetInstance().MyPlayerIndex == nowTurn ? true : false;
                            PlayerSystem.GetInstance().PlayerCardMoveDeck(DeckTag.DRAW_DECK, nowTurn, isMyTurn, 0.1f);
                            PlayerSystem.GetInstance().RemovePlayer(nowTurn);
                            DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 1000);
                            NextTurn(nowTurn, damage);
                            return;
                        }

                        Action addCardAction = () =>
                        {
                            DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 1000);
                            PlayerSystem.GetInstance().PlayerAddCardWithDeck(DeckTag.DRAW_DECK, nowTurn, damage);
                            NextTurn(nowTurn, damage);
                        };
                        
                        if (damage > DeckSystem.GetInstance().GetCardCountWithDeck(DeckTag.DRAW_DECK))
                        {
                            var count = DeckSystem.GetInstance().GetCardCountWithDeck(DeckTag.PUT_DECK);

                            DeckSystem.GetInstance().MoveCardDecktoDeck(
                                DeckTag.PUT_DECK,
                                DeckTag.DRAW_DECK,
                                0,
                                0,
                                count - 1,
                                1,
                                1,
                                addCardAction);
                        }
                        else
                        {
                            addCardAction();
                        }

                    });
            }
        });
    }

    public void NextTurn(int playerIdx, int damage)
    {
        if (damage != 0)
        {
            RuleSystem.GetInstance().IsAttackTurn = false;
            RuleSystem.GetInstance().SaveAttackDamage = 0;
        }

        TurnSystem.GetInstance().NextTurn();

        PlayerSystem.GetInstance().Players[playerIdx].IsPutCard = false;

        if (playerIdx == PlayerSystem.GetInstance().MyPlayerIndex)
        {
            Debug.Log("btn inter");
            Button btn = TurnEndBtnObject.GetComponent<Button>();
            btn.interactable = true;
        }
    }

    public void TouchUpdate()
    {
        if (PlayerSystem.GetInstance().Players.Count <= 0)
            return;

        Vector2 pos = Vector2.zero;
        int myPlayerIndex = PlayerSystem.GetInstance().MyPlayerIndex;

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
            PlayerSystem.GetInstance().Players[myPlayerIndex].SelectCard_Began(pos);
        }

        if (Input.GetMouseButton(0))
        {
            PlayerSystem.GetInstance().Players[myPlayerIndex].SelectCard_Moved(pos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            PlayerSystem.GetInstance().Players[myPlayerIndex].SelectCard_Ended();
        }
    }

    public IEnumerator StartScene()
    {
        yield return new WaitForSeconds(1);
        DeckSystem.GetInstance().AllMoveCardDecktoDeck(DeckTag.ANIMATION_RIGHT_DECK, DeckTag.DRAW_DECK, 0, 0.05f, 0.5f);
        yield return new WaitForSeconds(5);

        StartCoroutine(SetTurnDirection());
    }

    public IEnumerator SetTurnDirection()
    {
        DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 1000);

        //StartCoroutine(PlayerSystem.GetInstance().ShareCard(1, 0));
        //
        //yield return new WaitForSeconds(3);
        //
        //TurnSystem.GetInstance().DecideFirstTurn();
        //
        //PlayerSystem.GetInstance().AllPlayerCardReverse(0.5f);
        //
        //yield return new WaitForSeconds(2);
        //
        //PlayerSystem.GetInstance().AllPlayerCardMoveDeck(DeckTag.DRAW_DECK, true, 0.5f);
        //
        //yield return new WaitForSeconds(2);

        yield return StartCoroutine(GameManager.GetInstance().StartGame());
    }

    public override IEnumerator NextSceneAnimation()
    {
        yield return null;
    }


    #region PlayScene Btn Press Function

    public void TurnEndBtnPress()
    {
        if (TurnSystem.GetInstance().GetNowTurnPlayerIndex() ==
            PlayerSystem.GetInstance().MyPlayerIndex)
        {
            Button btn = TurnEndBtnObject.GetComponent<Button>();
            btn.interactable = false;

            TurnSystem.GetInstance().EndTurn();
        }
    }

    public void ChatBtnPress()
    {
        Animator[] obj = ChatBtnObject.GetComponentsInChildren<Animator>();

        for (int i = 0; i < obj.Length; i++)
        {
            if (IsChatBtnClick == false)
                obj[i].Play("Show");
            else
                obj[i].Play("Close");
        }

        IsChatBtnClick = !IsChatBtnClick;
    }

    public void EmotionExpressBtnPress(int motionNum)
    {
        ChatBtnPress();

        Animator[] obj = ChatImgObject.GetComponentsInChildren<Animator>();

        obj[motionNum].Play("Show");
    }

    public void OneCardBtnPress()
    {

    }

    public void MenuBtnPress()
    {

    }

    #endregion

}

