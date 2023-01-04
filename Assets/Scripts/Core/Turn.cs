using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turn
{
    List<Unit> units = new List<Unit>();
    private Action callback;

    public Turn(Action callback)
    {
        this.callback = callback;
    }

    public void AddUnit(Unit c)
    {
        units.Add(c);
    }

    public void Begin()
    {
        BattleController.Instance.StartCoroutine(TurnRoutine());
    }
    private IEnumerator TurnRoutine()
    {
        foreach (Unit unit in units)
        {
            BattleController.Instance.StartCoroutine(UnitTurnRoutine(unit));
        }
        while (units.Count > 0)
        {
            yield return null;
        }
        callback?.Invoke();


    }
    private IEnumerator UnitTurnRoutine(Unit unit)
    {
        yield return null;
        unit.pathmover.waitForCommand = false;
        while (!unit.MoveFinished())
        {
            yield return null; //Ждем следующего апдейта чтобы продолжить исполнение кода
        }

        if (unit.attackTarget != null)
        {
            for (int i = unit.stats.AttackCount; i > 0; i--)
            {
                unit.DoAttack(unit.attackTarget);
                yield return new WaitForSeconds(1.5f);
            }
            if (unit.attackTarget.stats.IsAlive())
            {
                for (int i = unit.attackTarget.stats.CounterAttackCount; i > 0; i--)
                {
                    unit.attackTarget.DoAttack(unit);
                    yield return new WaitForSeconds(1.5f);
                }
            }
            if (unit.stats.IsAlive())
            {
                unit.DoRetreat(unit.attackTarget);
            }
            if (unit.attackTarget.stats.IsAlive())
            {
                unit.attackTarget.DoRetreat(unit);
            }
        }
        units.Remove(unit);
    }
}

