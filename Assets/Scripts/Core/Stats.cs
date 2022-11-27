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
}
