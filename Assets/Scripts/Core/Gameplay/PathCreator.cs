using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private bool stopped = false;

    public void Stop()
    {
        stopped = true;
    }

    public void Continue()
    {
        stopped = false;
    }

    private bool IsPointNearSelectedUnit(Vector3 point)
    {
        if (stopped)
            return false;
        var selectedUnit = Game.Instance.BattleController.ActivePlayer.SelectedUnit;
        if (selectedUnit == null)
            return false;
        var dist = Vector3.Distance(selectedUnit.transform.position, point);
        return dist < 5;
    }
    
    private bool SelectedUnitHasEnoughEnergy(List<Vector3> p)
    {
        if (stopped)
            return false;
        float wayLength = 0;
        for (int i = 0; i < p.Count - 1; i++)
        {
            float dist = Vector3.Distance(p[i], p[i + 1]);
            wayLength += dist;
        }
        var selectedUnit = Game.Instance.BattleController.ActivePlayer.SelectedUnit;
        if (selectedUnit == null)
            return false;
        return wayLength < selectedUnit.MaxWayPerTurn(); //аналогия if проверки
    }

    // Start is called before the first frame update
    void Start()
    {
       // BattleController.Instance.onTurnBegin += LineReset;
    }

    // Update is called once per frame
    public void ManualUpdate(bool fireDown, bool fire, bool fireUp)
    {
        if (stopped)
            return;
        if (fireDown)
            points.Clear();

        if (fire)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (DistanceToLastPoint(hitInfo.point) > 1f)
                {
                    if ((points.Count > 0 || IsPointNearSelectedUnit(hitInfo.point)) && SelectedUnitHasEnoughEnergy(points))
                    {
                        points.Add(hitInfo.point);
                        OnNewPathCreated(points);

                    }


                }
            }

        }
        else if (fireUp)
            OnNewPathCreated(points);
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
            return Mathf.Infinity;
        return Vector3.Distance(points.Last(), point);
    }

      public void LineUpdate(Queue<Vector3> l, LineRenderer lineRenderer)
    {
       lineRenderer.positionCount = l.Count;
       lineRenderer.SetPositions(l.ToArray());
    }
}
