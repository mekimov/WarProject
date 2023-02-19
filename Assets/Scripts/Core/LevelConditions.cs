using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="NewLevelConditions", menuName ="WarProject/Level conditions")]

public class LevelConditions : ScriptableObject //конфиг
{
    [SerializeField] VictoryCondition victoryCondition;
    public VictoryCondition VictoryCondition => victoryCondition;
    [SerializeField] LooseCondition looseCondition;
    public LooseCondition LooseCondition => looseCondition;

}

public enum VictoryCondition
{
    KillTheKing = 1,
    KillAllUnits = 2,
}

public enum LooseCondition
{
    LooseAllUnits = 1,
    TurnsOut = 2,
    LooseTheKing = 3,
}