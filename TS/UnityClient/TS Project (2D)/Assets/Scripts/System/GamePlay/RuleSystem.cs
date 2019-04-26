using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSystem : SingletonBase<RuleSystem>
{
    public bool IsAttackTurn;
    public int SaveAttackDamage = 0;
    public int PlayerMaxCard = 20;

    public bool CompareCard(Card deckCard, Card putCard, bool isPutCard)
    {
        var deckShape = deckCard.GetShapeIndex();
        var deckCardIndex = deckCard.GetCardIndex();

        var putShape = putCard.GetShapeIndex();
        var putCardIndex = putCard.GetCardIndex();

        if (isPutCard)
        {
            if (deckCardIndex == putCardIndex ||
                deckCardIndex == CardTag.K && deckShape == putShape)
            {   
                if (putCardIndex == CardTag.Q)
                    TurnSystem.GetInstance().ReverseTurn();

                if (putCardIndex == CardTag.J)
                    TurnSystem.GetInstance().JumpTurn();

                return true;
            }

            if (deckCardIndex == CardTag.Joker || deckCardIndex == CardTag.JokerR)
            {
                if (putCardIndex == CardTag.Joker || putCardIndex == CardTag.JokerR)
                    return true;
            }

            return false;
        }

        if (CheckPutCardRule(deckCardIndex, CardTag.Joker, putShape, ShapeTag.Club, ShapeTag.Spade) ||
            CheckPutCardRule(deckCardIndex, CardTag.JokerR, putShape, ShapeTag.Heart, ShapeTag.Diamond) ||
            CheckPutCardRule(putCardIndex, CardTag.Joker, deckShape, ShapeTag.Club, ShapeTag.Spade) ||
            CheckPutCardRule(putCardIndex, CardTag.JokerR, deckShape, ShapeTag.Heart, ShapeTag.Diamond))
        {
            return true;
        }

        if (CheckAttackTurn(deckCardIndex, deckShape, putCardIndex, putShape))
        {
            if (deckShape == putShape || deckCardIndex == putCardIndex)
            {
                if (putCardIndex == CardTag.Q)
                    TurnSystem.GetInstance().ReverseTurn();
                
                if (putCardIndex == CardTag.J)
                    TurnSystem.GetInstance().JumpTurn();

                return true;
            }

            if (deckCardIndex == CardTag.Joker && putCardIndex == CardTag.JokerR ||
                deckCardIndex == CardTag.JokerR && putCardIndex == CardTag.Joker)
                return true;
        }

        return false;
    }

    private bool CheckPutCardRule(CardTag deckCard, CardTag ruleCard, ShapeTag putCardShape, ShapeTag ruleShape1, ShapeTag ruleShape2)
    {
        if (!IsAttackTurn)
            if (deckCard == ruleCard)
                if (putCardShape == ruleShape1 || putCardShape == ruleShape2)
                    return true;

        return false;
    }

    private bool CheckAttackTurn(CardTag deckCard, ShapeTag deckShape, CardTag putCard, ShapeTag putShape)
    {
        if (!IsAttackTurn)
            return true;

        if (deckCard == CardTag.A)
        {
            if (deckShape == ShapeTag.Spade && putCard == CardTag.Joker)
                return true;

            if (putCard == CardTag.A)
                return true;

            return false;
        }

        
        if (deckCard == CardTag.N2)
        {
            if (putCard == CardTag.N2)
                return true;

            if (putCard == CardTag.A && deckShape == putShape)
                return true;

            return false;
        }

        if (deckCard == CardTag.Joker || deckCard == CardTag.JokerR)
        {
            if (putCard == CardTag.Joker || putCard == CardTag.JokerR)
                return true;

            return false;
        }

        return true;
    }

    public bool CheckAttackCard(CardTag card)
    {
        if (card == CardTag.N2 || card == CardTag.A || card == CardTag.Joker || card == CardTag.JokerR)
            return true;

        return false;
    }

    public void AddAttackCardDamage(CardTag putCard, ShapeTag putShape)
    {
        if (putCard == CardTag.N2)
        {
            IsAttackTurn = true;
            SaveAttackDamage += 2;
        }

        else if (putCard == CardTag.A)
        {
            if (putShape == ShapeTag.Spade)
                SaveAttackDamage += 5;
            else
                SaveAttackDamage += 3;

            IsAttackTurn = true;
        }

        else if (putCard == CardTag.Joker)
        {
            IsAttackTurn = true;
            SaveAttackDamage += 5;
        }

        else if (putCard == CardTag.JokerR)
        {
            IsAttackTurn = true;
            SaveAttackDamage += 7;
        }
    }

    public int GetAttackDamage(bool isPutCard)
    {
        var damage = IsAttackTurn ? SaveAttackDamage : 1;

        if (!isPutCard)
            return damage;

        return 0;
    }

}

