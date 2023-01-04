using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { }; 

    private bool IsPointNearSelectedUnit(Vector3 point)
    {
        var selectedUnit = BattleController.Instance.ActivePlayer.SelectedUnit;
        if (selectedUnit == null)
            return false;
        var dist = Vector3.Distance(selectedUnit.transform.position, point);
        return dist < 5;
    }
    
    private bool SelectedUnitHasEnoughEnergy(List<Vector3> p)
    {
        float wayLength = 0;
        for (int i = 0; i < p.Count - 1; i++)
        {
            float dist = Vector3.Distance(p[i], p[i + 1]);
            wayLength += dist;
        }
        var selectedUnit = BattleController.Instance.ActivePlayer.SelectedUnit;
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
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            points.Clear();

        if (Input.GetButton("Fire1"))
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
        else if (Input.GetButtonUp("Fire1"))
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
