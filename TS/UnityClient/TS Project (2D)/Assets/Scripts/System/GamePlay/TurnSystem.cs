using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TurnSystem : SingletonBase<TurnSystem>
{
    public Queue<string> PlayerTurn = new Queue<string>();

    public string PlayerNowTurn;

    public float TurnTimeOut = 30;
    public float TurnNowTime;
    public float TurnShowTime = 15;
    public bool IsShowTimeBar = false;

    public void Start()
    {

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
                IsShowTimeBar = true;
            }

            yield return null;
        }

        IsShowTimeBar = false;
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

