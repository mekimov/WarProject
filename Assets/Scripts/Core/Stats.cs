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
    public int Initiative { get { return initiative; } }
    [SerializeField] private int attackCount = 1;
    public int AttackCount { get { return attackCount; } }
    [SerializeField] private int counterAttackCount = 1;
    public int CounterAttackCount { get { return counterAttackCount; } }
}
