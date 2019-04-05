using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;


public class TurnSystem : SingletonBase<TurnSystem>
{
    public Queue<string> PlayerTurn = new Queue<string>();

    

    public float TurnTimeOut = 30;
    public float TurnNowTime;
    public float TurnShowTime = 7;
    public StringReactiveProperty PlayerNowTurn;
    public BoolReactiveProperty IsShowTimeBar = new BoolReactiveProperty(false);
    public BoolReactiveProperty IsFinishTurn = new BoolReactiveProperty(false);

    public Image TimeObject;
    public Image TimeBackObject;


    public bool IsStartTurn;

    public void Start()
    {
        TimeObject.canvasRenderer.SetAlpha(0);
        TimeBackObject.canvasRenderer.SetAlpha(0);

        IsShowTimeBar.Subscribe(value =>
        {
            //PlayerTurn.Reverse();
            if (value)
            {
                TimeBackObject.CrossFadeAlpha(0.6f, 2, false);
                TimeObject.CrossFadeAlpha(1, 2, false);

                var pos = TimeBackObject.transform.position;
                TimeBackObject.transform.DOMove(new Vector3(pos.x, pos.y + 30), 1);

                pos = TimeObject.transform.position;
                TimeObject.transform.DOMove(new Vector3(pos.x, pos.y + 30), 1);
            }
            else
            {
                TimeBackObject.CrossFadeAlpha(0, 1, false);
                TimeObject.CrossFadeAlpha(0, 1, false);

                TimeBackObject.transform.DOScale(TimeBackObject.transform.localScale * 2, 1);
                TimeObject.transform.DOScale(TimeObject.transform.localScale * 2, 1)
                    .OnComplete(() =>
                    {
                        TimeBackObject.transform.Translate(0, -30, 0);
                        TimeObject.transform.Translate(0, -30, 0);

                        TimeBackObject.transform.localScale /= 2;
                        TimeObject.transform.localScale /= 2;
                    });
            }
        });

        this.UpdateAsObservable().Subscribe(_ =>
        {
            var scale = TurnNowTime / TurnShowTime;
            TimeObject.fillAmount = scale;

            TurnUpdate();
        });

    }

    /// <summary>Turn Queue에 플레이어 추가 </summary>
    /// <param name="playerName">플레이어 이름</param>
    public void AddTurnPlayer(string playerName)
    {
        PlayerTurn.Enqueue(playerName);
    }

    /// <summary>턴을 시작하는 함수 </summary>
    public void StartTurn()
    {
        IsStartTurn = true;
        TurnNowTime = TurnTimeOut;
        IsFinishTurn.Value = false;
    }

    /// <summary>다음 턴으로 넘기는 함수 </summary>
    public void NextTurn()
    {
        PlayerNowTurn.Value = PlayerTurn.Peek();
        TurnNowTime = TurnTimeOut;
        IsFinishTurn.Value = false;
        NetworkSystem.GetInstance().SendServer("NEXT-TURN");
    }

    /// <summary>현재 턴을 종료하는 함수 </summary>
    public void EndTurn()
    {
        PlayerTurn.Enqueue(PlayerTurn.Dequeue());
        TurnNowTime = 0;
        IsShowTimeBar.Value = false;
        IsFinishTurn.Value = true;
    }

    /// <summary>턴을 하나 건너뜀 </summary>
    public void JumpTurn()
    {
        PlayerTurn.Enqueue(PlayerTurn.Dequeue());
    }

    /// <summary>턴을 반대로 바꿈 </summary>
    public void ReverseTurn()
    {
        //PlayerTurn.Enqueue(PlayerTurn.Dequeue());
        PlayerTurn = new Queue<string>(PlayerTurn.Reverse());
    }

    /// <summary>첫 번째 턴을 결정하는 함수 </summary>
    public void DecideFirstTurn()
    {
        int nowCardScore = 0;
        string playerName = null;

        for (int i = 0; i < PlayerSystem.GetInstance().Players.Count; i++)
        {
            int score = PlayerSystem.GetInstance().Players[i].PlayerCard[0].GetCardScore();

            if (score > nowCardScore)
            {
                playerName = PlayerSystem.GetInstance().Players[i].PlayerId;
                nowCardScore = score;
            }
        }

        SetFirstTurn(playerName);
    }

    /// <summary>Turn Queue에서 해당 플레이어를 첫 번째로 배정하는 함수 </summary>
    /// <param name="playerName">플레이어 이름</param>
    public void SetFirstTurn(string playerName)
    {
        while (!PlayerTurn.Peek().Equals(playerName))
            PlayerTurn.Enqueue(PlayerTurn.Dequeue());

        PlayerNowTurn.Value = PlayerTurn.Peek();
    }

    /// <summary>현재 턴의 플레이어 index 반환 함수</summary>
    /// <returns>현재 턴의 플레이어 index</returns>
    public int GetNowTurnPlayerIndex()
    {
        int count = PlayerSystem.GetInstance().Players.Count;

        if (count <= 0)
            return -1;

        for (int i = 0; i < count; i++)
        {
            if (PlayerNowTurn.Value.Equals(PlayerSystem.GetInstance().Players[i].PlayerId))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>턴의 타이머 업데이트 함수</summary>
    private void TurnUpdate()
    {
        if (!IsFinishTurn.Value && GameManager.GetInstance().IsStartGame)
        {
            TurnNowTime -= Time.deltaTime;

            if (TurnNowTime < TurnShowTime)
            {
                IsShowTimeBar.Value = true;
            }

            if (TurnNowTime <= 0)
            {
                EndTurn();
            }
        }
    }
}

