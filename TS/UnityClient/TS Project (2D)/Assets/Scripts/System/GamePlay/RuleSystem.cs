using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSystem : SingletonBase<RuleSystem>
{
    public bool IsAttackTurn;
    public int SaveAttackDamage = 0;

    // TODO: 공격카드 먹게 고쳐야함

    public bool CompareCard(Card deckCard, Card putCard, bool isPutCard)
    {
        var deckShape = deckCard.GetShapeIndex();
        var deckCardIndex = deckCard.GetCardIndex();

        var putShape = putCard.GetShapeIndex();
        var putCardTag = putCard.GetCardIndex();

        CheckAttackCardRule(putCardTag, putShape);

        if (CheckAttackTurn(deckCardIndex, deckShape, putCardTag, putShape))
            return true;

        if (isPutCard)
        {
            if (deckCardIndex == putCardTag ||
                deckCardIndex == CardTag.K)
            {
                return true;
            }

            return false;
        }

        if (CheckPutCardRule(deckCardIndex, putShape, CardTag.Joker, ShapeTag.Club, ShapeTag.Spade) ||
            CheckPutCardRule(deckCardIndex, putShape, CardTag.JokerR, ShapeTag.Heart, ShapeTag.Diamond) ||
            CheckPutCardRule(putCardTag, deckShape, CardTag.Joker, ShapeTag.Club, ShapeTag.Spade) ||
            CheckPutCardRule(putCardTag, deckShape, CardTag.JokerR, ShapeTag.Heart, ShapeTag.Diamond))
        {
            return true;
        }

        if (deckShape == putShape || deckCardIndex == putCardTag)
            return true;

        return false;
    }

    private bool CheckPutCardRule(CardTag deckCard, ShapeTag putCardShape, CardTag ruleCard, ShapeTag ruleShape1, ShapeTag ruleShape2)
    {
        if (deckCard == ruleCard)
            if (putCardShape == ruleShape1 || putCardShape == ruleShape2)
                return true;

        return false;
    }

    private bool CheckAttackTurn(CardTag deckCard, ShapeTag deckShape, CardTag putCard, ShapeTag putShape)
    {
        if (!IsAttackTurn)
            return false;

        if (deckCard == CardTag.A && deckShape == ShapeTag.Spade)
        {
            if (putCard == CardTag.Joker)
                return true;

            return false;
        }

        if (deckCard == CardTag.N2)
        {
            if (putCard == CardTag.N2)
                return true;

            if (putCard == CardTag.A && deckShape == putShape)
                return true;
        }

        if (deckCard == putCard)
        {
            return true;
        }

        return false;
    }

    private void CheckAttackCardRule(CardTag putCard, ShapeTag putShape)
    {
        Debug.Log("Attack turn on");
        IsAttackTurn = true;

        if (putCard == CardTag.N2)
            SaveAttackDamage += 2;

        else if (putCard == CardTag.A)
        {
            if (putShape == ShapeTag.Spade)
                SaveAttackDamage += 5;
            else
                SaveAttackDamage += 3;
        }

        else if (putCard == CardTag.Joker)
            SaveAttackDamage += 5;
        else if (putCard == CardTag.JokerR)
            SaveAttackDamage += 7;

        else
        {
            Debug.Log("Attack turn off");
            IsAttackTurn = false;
        }
            
    }
}

