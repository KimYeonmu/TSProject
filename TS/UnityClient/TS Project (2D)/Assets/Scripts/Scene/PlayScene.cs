using System;
using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Video;

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

        AiSystem.GetInstance().SetupOneCardAi();

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


            //NetworkSystem.GetInstance().SendServer("FIND-ROOM:" + "1");
        }

        StartCoroutine(StartScene());

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!GameManager.GetInstance().IsStartGame)
                    return;

                var nowId = TurnSystem.GetInstance().PlayerNowTurn.Value;

                if (nowId == PlayerSystem.GetInstance().MyPlayerId)
                    InputSystem.GetInstance().TouchUpdate();
            });

        TurnSystem.GetInstance().PlayerNowTurn.Subscribe(name =>
        {
            if (name == "")
                return;

            if (PlayerSystem.GetInstance().GetPlayer(name).IsAi)
                AiSystem.GetInstance().IsStartAi.Value = true;

            if (name == PlayerSystem.GetInstance().MyPlayerId)
                OnTurnEndBtn();
        });

        TurnSystem.GetInstance().IsFinishTurn.Subscribe(finish =>
        {
            if (!GameManager.GetInstance().IsStartGame)
                return;

            if (finish)
            {
                Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
                    {
                        if (GameManager.GetInstance().CheckDefeatPlayer())
                            return;

                        if (GameManager.GetInstance().CheckWinPlayer())
                            return;

                        GameManager.GetInstance().FillDecktoDeckofAttack(DeckTag.PUT_DECK, DeckTag.DRAW_DECK);
                    });
            }
        });
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


    #region PlayScene Btn Function

    public void OnTurnEndBtn()
    {
        Debug.Log("btn inter");
        Button btn = TurnEndBtnObject.GetComponent<Button>();
        btn.interactable = true;
    }

    public void TurnEndBtnPress()
    {
        var nowId = TurnSystem.GetInstance().PlayerNowTurn.Value;

        if (nowId == PlayerSystem.GetInstance().MyPlayerId)
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
        var btn = OneCardBtnObject.GetComponent<Button>();
        var myId = PlayerSystem.GetInstance().MyPlayerId;

        btn.interactable = false;


        Observable.Timer(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => { btn.interactable = true; });


        foreach (var player in PlayerSystem.GetInstance().Players)
        {
            if (player.PlayerCard.Count == 1)
            {
                GameManager.GetInstance().FillDecktoDeckofDraw(DeckTag.PUT_DECK, DeckTag.DRAW_DECK, 1);
                PlayerSystem.GetInstance().PlayerAddCardWithDeck(DeckTag.DRAW_DECK, player.PlayerId, 1);
                return;
            }
        }

        GameManager.GetInstance().FillDecktoDeckofDraw(DeckTag.PUT_DECK, DeckTag.DRAW_DECK, 1);
        PlayerSystem.GetInstance().PlayerAddCardWithDeck(DeckTag.DRAW_DECK, myId, 1);
    }

    public void MenuBtnPress()
    {

    }

    #endregion

}

