using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turn
{
    private class UnitTurnInfo
    {
        public int counterAttacksCount = 0;
        public Coroutine turnRoutine;
    }

    Dictionary<Unit, UnitTurnInfo> dictionary = new Dictionary<Unit, UnitTurnInfo>();
    List<Unit> units = new List<Unit>();
    private Action callback;

    public Turn(Action callback)
    {
        this.callback = callback;
    }

    private UnitTurnInfo GetUnitTurnInfo(Unit u)
    {
        if (dictionary.ContainsKey(u))
            return dictionary[u];
        var uti = new UnitTurnInfo();
        dictionary.Add(u, uti);
        return uti;
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
            var c = BattleController.Instance.StartCoroutine(UnitTurnRoutine(unit));
            GetUnitTurnInfo(unit).turnRoutine = c;
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
            BattleController.Instance.StartCoroutine(FightRoutine(unit, unit.attackTarget));
        }
        units.Remove(unit);
    }

    private IEnumerator FightRoutine(Unit unit1, Unit unit2)
    {
        var r1 = GetUnitTurnInfo(unit1).turnRoutine;
        BattleController.Instance.StopCoroutine(r1);
        var r2 = GetUnitTurnInfo(unit2).turnRoutine;
        BattleController.Instance.StopCoroutine(r2);
        Unit unitFirst;
        Unit unitSecond;
        if (GetAttackPriority(unit1) > GetAttackPriority(unit2))
        {
            unitFirst = unit1;
            unitSecond = unit2;
        }
        else if (GetAttackPriority(unit1) < GetAttackPriority(unit2))
        {
            unitFirst = unit2;
            unitSecond = unit1;
        }
        else
        {
            if (UnityEngine.Random.Range(0,999)%2 == 1)
            {
                unitFirst = unit1;
                unitSecond = unit2;
            }
            else
            {
                unitFirst = unit2;
                unitSecond = unit1;
            }
        }
 
        var uti = GetUnitTurnInfo(unitSecond);
        for (int i = unitFirst.stats.AttackCount; i > 0; i--)
        {
            if (GetAttackPriority(unitFirst) > 0)
            {
                unitFirst.DoAttack(unitSecond);
                yield return new WaitForSeconds(1.5f);
            }
            if (unitSecond.stats.IsAlive())
            {
                if (uti.counterAttacksCount < unitSecond.stats.CounterAttackCount && GetAttackPriority(unitSecond) > 0)
                {
                    unitSecond.DoAttack(unitFirst);
                    yield return new WaitForSeconds(1.5f);
                    uti.counterAttacksCount++;
                }
            }
        }
        if (unitFirst.stats.IsAlive())
        {
            unitFirst.DoRetreat(unitSecond);
        }
        if (unitSecond.stats.IsAlive())
        {
            unitSecond.DoRetreat(unitFirst);
        }
        units.Remove(unitFirst);
        units.Remove(unitSecond);

    }

    private int GetAttackPriority(Unit unit)
    {
        return unit.stats.Initiative;
    }
}

