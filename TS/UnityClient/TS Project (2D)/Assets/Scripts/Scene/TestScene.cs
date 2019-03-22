using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestScene : IScene
{
    void Start()
    {
        StartCoroutine(Test());
    }

    public override IEnumerator NextSceneAnimation()
    {
        yield return null;
    }

    public IEnumerator Test()
    {
        yield return new WaitForSeconds(2);

        PlayerSystem.GetInstance().AddPlayer(PlayerTag.PLAYER_BOTTOM, "test", false);

        TurnSystem.GetInstance().SetFirstTurn("test");


        yield return new WaitForSeconds(2);

        DeckSystem.GetInstance().AllMoveCardDecktoDeck(
            DeckTag.ANIMATION_RIGHT_DECK,
            DeckTag.DRAW_DECK,
            3,
            0.05F,
            3);
    }
}

