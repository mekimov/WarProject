using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : Player
{
    [SerializeField] private List<BotUnitPatrolRecord> unitPatrols;
    [SerializeField] float agroDistance = 15f;
    int pointIndex = 0;
    protected override void ProcessInput()
    {
        
    }

    protected override void ProcessBeginTurn()
    {
        StartCoroutine(ProcessPatrol());
    }

    private IEnumerator ProcessPatrol()
    {
        //заблокировать кнопку в бой (button.Interactable)
        yield return new WaitForSeconds(2);
        foreach (var patrol in unitPatrols)
        {
            var points = new List<Vector3>();
            points.Add(patrol.unit.transform.position);
            var nextPoint = patrol.waypoints[pointIndex % patrol.waypoints.Count].position;
            if (Physics.Raycast(nextPoint, Vector3.down, out var hit))
                nextPoint = hit.point;
            var targetUnit = GetClosestEnemy(patrol.unit);
            if (targetUnit != null)
                points.Add(targetUnit.transform.position);
            else
                points.Add(nextPoint);
            patrol.unit.pathmover.SetPoints(points);
            yield return new WaitForSeconds(0.5f);
        }
        pointIndex++;
        //Разблокировать кнопку в бой
        Game.Instance.BeginFight();
    }
    private Unit GetClosestEnemy(Unit owner)
    {
        if (owner.UnitType == UnitType.King)
            return null;
        var minDist = float.MaxValue;
        Unit target = null;
        foreach (var unit in Game.Instance.BattleController.HumanPlayer.AllUnits)
        {
            var dist = Vector3.Distance(owner.transform.position, unit.transform.position);
            if (dist < minDist && dist < agroDistance)
            {
                minDist = dist;
                target = unit;
            }
        }
        return target;
    }
}

[System.Serializable] public class BotUnitPatrolRecord
{
    public Unit unit;
    public List<Transform> waypoints;
}
