using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : Player
{
    [SerializeField] private List<BotUnitPatrolRecord> unitPatrols;
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
        Debug.LogError("Process patrol begins");
        yield return new WaitForSeconds(2);
        foreach (var patrol in unitPatrols)
        {
            var points = new List<Vector3>();
            points.Add(patrol.waypoints[pointIndex % patrol.waypoints.Count].position);
            patrol.unit.pathmover.SetPoints(points);
            yield return new WaitForSeconds(0.5f);
        }
        pointIndex++;
        Debug.LogError("Process patrol finished");
    }
}

[System.Serializable] public class BotUnitPatrolRecord
{
    public Unit unit;
    public List<Transform> waypoints;
}
