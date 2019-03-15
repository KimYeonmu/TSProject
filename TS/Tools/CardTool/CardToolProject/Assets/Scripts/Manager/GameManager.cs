using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : SingletonBase<GameManager>
{
    private List<Player> _players = new List<Player>();

    public Deck DrawDeck;
    public Deck PutDeck;

    public GameObject RootObject;

    public int MyPlayerIndex;
    
    void Awake()
    {
        DrawDeck = RootObject.AddComponent<Deck>();
        DrawDeck.SetParent(RootObject.transform);
        DrawDeck.SetPosition(Vector2.up * 0.75f);
        DrawDeck.SetScale(Vector2.one);

        PutDeck = RootObject.AddComponent<Deck>();
        PutDeck.SetParent(RootObject.transform);
        PutDeck.SetPosition(Vector2.down * 0.4f);
        PutDeck.SetScale(Vector2.one);
    }

    // Use this for initialization
    void Start()
    {
        CardSystem.GetInstance().AddAllCard();

        for (int i = 0; i < CardSystem.GetInstance().Cards.Count; i++)
        {
            DrawDeck.DeckList.Add(CardSystem.GetInstance().Cards[i]);
            CardSystem.GetInstance().Cards[i].SetParent(DrawDeck.Object.transform);
            CardSystem.GetInstance().Cards[i].SetPosition(DrawDeck.GetPosition());
        }

        StartCoroutine(StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        TouchUpdate();
        
    }

    public void TouchUpdate()
    {
        Vector2 pos = Vector2.zero;

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            _players[MyPlayerIndex].SelectCard_Began(pos);
        }

        if (Input.GetMouseButton(0))
        {
            _players[MyPlayerIndex].SelectCard_Moved(pos);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _players[MyPlayerIndex].SelectCard_Ended();
        }
    }


    public void AddPlayer(PlayerTag tag, bool isAi)
    {
        Player player = gameObject.AddComponent<Player>();
        player.PlayerPlace = tag;
        player.IsAi = isAi;

        _players.Add(player);
    }

    public IEnumerator StartScene()
    {
        this.AddPlayer(PlayerTag.PLAYER_BOTTOM,     false);

        this.AddPlayer(PlayerTag.PLAYER_TOP,        true);

        yield return new WaitForSeconds(3);
    }

    public IEnumerator AddCard(Deck cardDeck, int firstCardIndex, int lastCardIndex, int playerIndex)
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        while (firstCardIndex < lastCardIndex)
        {
            _players[playerIndex].AddPlayerCard(cardDeck.GetTopCard());
            _players[playerIndex].RePosition();
            firstCardIndex++;
            yield return delay;
        }
    }

    public IEnumerator ShareCard(Deck cardDeck, int cardNum, float shareDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(shareDelay);

        for (int i = 0; i < _players.Count; i++)
        {
            for (int j = 0; j < cardNum; j++)
            {
                Card card = cardDeck.GetTopCard();
                _players[i].AddPlayerCard(card);
            }

            _players[i].RePosition();
            yield return delay;
        }


        for (int j = 0; j < _players[MyPlayerIndex].PlayerCard.Count; j++)
        {
            _players[MyPlayerIndex].PlayerCard[j].Object.AddComponent<BoxCollider2D>();
        }
    }

    public void AddCard()
    {
        
    }

    public void ShffleDeck()
    {
        DrawDeck.ShuffleDeck(200);
    }
}