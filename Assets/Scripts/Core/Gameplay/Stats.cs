using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int stamina = 50;
    [SerializeField] private int maxHP = 100;
    public int MaxHP { get { return maxHP; } }
    [SerializeField] private int currentHP = 100;
    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
    [SerializeField] private int attack = 100;
    public int Attack { get { return attack; } }
    [SerializeField] private int initiative = 100;
    public int Initiative { get { return initiative + _initiativeModifier; } }
    [SerializeField] private int attackCount = 1;
    public int AttackCount { get { return attackCount; } }
    [SerializeField] private int counterAttackCount = 1;
    public int CounterAttackCount { get { return counterAttackCount; } }
    [SerializeField] private int priority = 100;
    public int Priority { get { return priority; } }

    private int _initiativeModifier = 0;

    public bool IsAlive()
    {
        return CurrentHP > 0f;
    }

    public void SetHitDirection(Side from, Side to)
    {
        if (from == Side.Back)
            _initiativeModifier = -999999;
        if (from == Side.Front)
            _initiativeModifier = 1000000;
        if (from == Side.Left || from == Side.Right)
            _initiativeModifier = 100000;
    }
}
