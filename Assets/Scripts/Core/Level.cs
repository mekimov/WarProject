using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] LevelConditions levelConditions;
    // Start is called before the first frame update
    void Start()
    {
        if (levelConditions == null)
        {
            Debug.LogError("ERROR! No level conditions set!");
        }
        Game.Instance.SetLevel(this);
        Game.Instance.EventBus.onUnitKilled += CheckLevelConditionsOnUnitKilled;
    }

    public void CheckLevelConditionsOnUnitKilled(Unit unit)
    {
        if (CheckWin(unit))
        {
            Game.Instance.EventBus.OnPlayerWin();
        }
        else if (CheckLoose(unit))
        {
            Game.Instance.EventBus.OnPlayerLoose();
        }
    }

    public bool CheckWin(Unit unit)
    {
        switch (levelConditions.VictoryCondition)
        {
            case VictoryCondition.KillTheKing:
                if (unit.UnitType == UnitType.King)
                    return true;
                break;
            case VictoryCondition.KillAllUnits:
                throw new System.NotImplementedException();
            default:
                throw new System.ArgumentException();

        }
           

        return false;
    }
    public bool CheckLoose(Unit unit)
    {
        switch (levelConditions.LooseCondition)
        {
            case LooseCondition.LooseAllUnits:
                if (Game.Instance.BattleController.HumanPlayer.AllUnits.Count == 0)
                    return true;
                break;
            case LooseCondition.LooseTheKing:
                throw new System.NotImplementedException();
            case LooseCondition.TurnsOut:
                throw new System.NotImplementedException();
            default:
                throw new System.ArgumentException();

        }

        return false;

    }
}
