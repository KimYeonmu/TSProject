using System;
using System.Collections;
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
        GameManager.GetInstance().FadeOutWhiteImg(0.5f);

        //SceneCanvasGroup.alpha = 0;
        //
        //StartCoroutine(AnimationSystem.GetInstance().FadeOutAnimation(SceneCanvasGroup, 1));

        base.Awake();

        //Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        Screen.SetResolution(720, 1280, false);
        //SceneSystem.GetInstance().SetScenePos(new Vector2(0, -20));
        //SceneSystem.GetInstance().SetSceneScale(Vector2.zero);

        var size = GameObject.FindWithTag("Background").GetComponent<SpriteRenderer>().sprite.bounds.size;

        SceneSystem.GetInstance().SetScreenSize(new Vector2(size.x, size.y));
    }

    void Start()
    {
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

            var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

            if (nowTurn != PlayerSystem.GetInstance().MyPlayerIndex)
                AiSystem.GetInstance().IsStartAi.Value = true;
                
        });

        TurnSystem.GetInstance().IsFinishTurn.Subscribe(finish =>
        {
            if (!GameManager.GetInstance().IsStartGame)
                return;

            if (finish && GameManager.GetInstance().IsStartGame)
            {
                var nowTurn = TurnSystem.GetInstance().GetNowTurnPlayerIndex();

                Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
                    {
                        if (PlayerSystem.GetInstance().CheckPutCardNowTurn(nowTurn,
                            RuleSystem.GetInstance().IsAttackTurn,
                            RuleSystem.GetInstance().SaveAttackDamage))
                        {
                            RuleSystem.GetInstance().IsAttackTurn = false;
                            RuleSystem.GetInstance().SaveAttackDamage = 0;
                        }

                        TurnSystem.GetInstance().NextTurn();

                        if (nowTurn == PlayerSystem.GetInstance().MyPlayerIndex)
                        {
                            Button btn = TurnEndBtnObject.GetComponent<Button>();
                            btn.interactable = true;
                        }
                    });
            }
        });
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
        DeckSystem.GetInstance().ShuffleDeck(DeckTag.DRAW_DECK, 50);

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
}

