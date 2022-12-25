using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turn
{
    List<Unit> characters = new List<Unit>();
    private Action callback;

    public Turn(Action callback)
    {
        this.callback = callback;
    }

    public void AddUnit(Unit c)
    {
        characters.Add(c);
    }

    public void Begin()
    {
        BattleController.Instance.StartCoroutine(TurnRoutine());
    }
    private IEnumerator TurnRoutine()
    {
        foreach (Unit unit in characters)
        {
            BattleController.Instance.StartCoroutine(UnitTurnRoutine(unit));
        }
        while (characters.Count > 0)
        {
            yield return null;
        }
        callback?.Invoke();


    }
    private IEnumerator UnitTurnRoutine(Unit character)
    {
        yield return null;
        character.pathmover.waitForCommand = false;
        while (!character.MoveFinished())
        {
            yield return null; //Ждем следующего апдейта чтобы продолжить исполнение кода
        }

        if (character.attackTarget != null)
        {
            character.DoAttack(character.attackTarget);
            yield return new WaitForSeconds(1.5f);
            if (character.attackTarget.stats.IsAlive())
            {
                character.attackTarget.DoAttack(character);
                yield return new WaitForSeconds(1.5f);
            }
            if (character.stats.IsAlive())
            {
                character.DoRetreat(character.attackTarget);
            }
            if (character.attackTarget.stats.IsAlive())
            {
                character.attackTarget.DoRetreat(character);
            }
        }
        characters.Remove(character);
    }
}

