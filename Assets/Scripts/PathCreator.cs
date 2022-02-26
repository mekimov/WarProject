using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { }; // вот тут ниче не понял

    private bool IsPointNearSelectedCharacter(Vector3 point)
    {
        var selectedCharacter = BattleController.Instance.ActivePlayer.SelectedCharacter;
        if (selectedCharacter == null)
            return false;
        var dist = Vector3.Distance(selectedCharacter.transform.position, point);
        return dist < 5;
    }
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            points.Clear();

        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // вот тут ниче не понял
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (DistanceToLastPoint(hitInfo.point) > 1f)
                {
                    if (points.Count > 0 || IsPointNearSelectedCharacter(hitInfo.point))
                    {
                        points.Add(hitInfo.point);
                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPositions(points.ToArray());
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
}
