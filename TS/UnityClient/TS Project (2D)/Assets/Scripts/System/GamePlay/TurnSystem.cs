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

    public string PlayerNowTurn;

    public float TurnTimeOut = 30;
    public float TurnNowTime;
    public float TurnShowTime = 7;
    public BoolReactiveProperty IsShowTimeBar = new BoolReactiveProperty(false);

    public Image TimeObject;
    public Image TimeBackObject;

    private float _timeObjectX;

    public void Start()
    {
        _timeObjectX = TimeObject.rectTransform.sizeDelta.x;

        TimeObject.canvasRenderer.SetAlpha(0);
        TimeBackObject.canvasRenderer.SetAlpha(0);

        IsShowTimeBar.Subscribe(value =>
        {
            if (value)
            {
                TimeBackObject.CrossFadeAlpha(0.6f, 2, false);
                TimeObject.CrossFadeAlpha(1, 2, false);

                var pos = TimeBackObject.transform.position;
                TimeBackObject.transform.DOMove(new Vector3(pos.x,pos.y + 30), 1);

                pos = TimeObject.transform.position;
                TimeObject.transform.DOMove(new Vector3(pos.x,pos.y + 30), 1);
            }
            else
            {
                TimeBackObject.CrossFadeAlpha(0, 1, false);
                TimeObject.CrossFadeAlpha(0, 1, false);

                TimeBackObject.transform.DOScale(TimeBackObject.transform.localScale * 2, 0.5f);
                TimeObject.transform.DOScale(TimeObject.transform.localScale * 2, 0.5f)
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
            var r = TimeObject.rectTransform;
            var scale = TurnNowTime / TurnShowTime;

            r.sizeDelta = new Vector2(_timeObjectX * scale, r.sizeDelta.y);
        });
        
    }

    public void AddTurnPlayer(string playerName)
    {
        PlayerTurn.Enqueue(playerName);
    }

    public void NextTurn()
    {
        PlayerTurn.Enqueue(PlayerTurn.Dequeue());

        PlayerNowTurn = PlayerTurn.Peek();

        TurnNowTime = TurnTimeOut;

        StopCoroutine(StartTurn());
        StartCoroutine(StartTurn());
    }

    public void SetFirstTurn(string playerName)
    {
        while(!PlayerTurn.Peek().Equals(playerName))
            PlayerTurn.Enqueue(PlayerTurn.Dequeue());

        PlayerNowTurn = PlayerTurn.Peek();
    }

    public IEnumerator StartTurn()
    {
        while (TurnNowTime > 0)
        {
            TurnNowTime -= Time.deltaTime;

            if (TurnNowTime < TurnShowTime)
            {
                IsShowTimeBar.Value = true;
            }

            yield return null;
        }

        IsShowTimeBar.Value = false;
        TurnNowTime = TurnTimeOut;

        NextTurn();
    }

    public void DecideFirstTurn()
    {
        int nowCardScore = 0;
        string playerName = null;

        for (int i = 0; i < PlayerSystem.GetInstance().Players.Count; i++)
        {
            if (PlayerSystem.GetInstance().Players[i].PlayerCard[0].GetCardScore() > nowCardScore)
            {
                playerName = PlayerSystem.GetInstance().Players[i].PlayerId;
                nowCardScore = PlayerSystem.GetInstance().Players[i].PlayerCard[0].GetCardScore();
            }
        }

       SetFirstTurn(playerName);
    }

    public int GetNowTurnPlayerIndex()
    {
        int count = PlayerSystem.GetInstance().Players.Count;

        for (int i = 0; i < count; i++)
        {
            if (PlayerNowTurn.Equals(PlayerSystem.GetInstance().Players[i].PlayerId))
            {
                return i;
            }
        }

        return -1;
    }
}

