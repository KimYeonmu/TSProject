using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : SingletonBase<GameManager>
{
    public bool IsStartGame = false;
    public Image WhiteImage;

    void Awake()
    {
        WhiteImage = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        //NetworkSystem.GetInstance().StartConnect();
    }

    public IEnumerator StartGame()
    {
        StartCoroutine(PlayerSystem.GetInstance().ShareCard(DeckTag.DRAW_DECK, 12, 0.1f));

        yield return new WaitForSeconds(PlayerSystem.GetInstance().Players.Count * 1);

        DeckSystem.GetInstance().MoveCardDecktoDeck(DeckTag.DRAW_DECK, DeckTag.PUT_DECK,3, 1, 0, 0.5f);

        yield return new WaitForSeconds(1);

        PlayerSystem.GetInstance().PlayerCardReverse(PlayerSystem.GetInstance().MyPlayerIndex, 0.5f);

        yield return new WaitForSeconds(1);

        IsStartGame = true;

        TurnSystem.GetInstance().StartTurn();
    }

    public void FadeInWhiteImg(float duration)
    {
        WhiteImage.color = new Color(1,1,1,0);
        WhiteImage.gameObject.SetActive(true);
        WhiteImage.DOColor(Color.white, duration);
    }

    public void FadeOutWhiteImg(float duration)
    {
        WhiteImage.color = Color.white;
        WhiteImage.gameObject.SetActive(true);
        WhiteImage.DOColor(new Color(1, 1, 1, 0), duration)
            .OnComplete(()=>WhiteImage.gameObject.SetActive(false));
    }
}
