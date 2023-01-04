using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PathMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navmeshagent;
    [SerializeField] private Animator animator;
    [SerializeField] private LineRenderer lineRenderer;

    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    Vector3 prevPosition;

    public delegate void MethodContainer(Queue<Vector3> pathUpdate);
    public event MethodContainer onLineDeque;

    //public Action<Queue> actionRedrawLine;
    
    public bool waitForCommand; 

    public void OnDrawGizmos()
    {
        foreach (var point in pathPoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(point, 2);
        }
    }
    public void OnSelect()
    {
        //navmeshagent = GetComponent<NavMeshAgent>(); делаем сериализацией поля
        prevPosition = transform.position;
        FindObjectOfType<PathCreator>().OnNewPathCreated += SetPoints;
    }
    public void OnUnselect()
    {
        FindObjectOfType<PathCreator>().OnNewPathCreated -= SetPoints;

    }

    public void ResetPath()
    {
        pathPoints.Clear();
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        if (pathPoints.Count > 0 && points.Count() == 0)
            return;
        lineRenderer.positionCount = points.Count();
        lineRenderer.SetPositions(points.ToArray());
        pathPoints = new Queue<Vector3>(points); 
        //if (pathPoints.Count > 0)
            //waitForCommand = false;
    }

       // Update is called once per frame
    public void Update()
    {
        UpdatePathing();
        animator.SetFloat("MoveSpeed", (transform.position - prevPosition).magnitude / Time.deltaTime);
        prevPosition = transform.position;
    }

    private void UpdatePathing()
    {
        if (ShouldSetDestination())
        {
            navmeshagent.SetDestination(pathPoints.Dequeue());
            onLineDeque?.Invoke(pathPoints);
            FindObjectOfType<PathCreator>().LineUpdate(pathPoints, lineRenderer);
        }
    }

    private bool ShouldSetDestination()
    {
        if (pathPoints.Count == 0)
            return false;
        if (waitForCommand)
            return false;
        if (navmeshagent.hasPath == false || navmeshagent.remainingDistance < 1f)
            return true;
        return false;
    }

    public bool MoveFinished()
    {
        return (pathPoints.Count == 0 && !waitForCommand);
    }

    public void StopMoving()
    {
        pathPoints.Clear();
        waitForCommand = false;
        onLineDeque?.Invoke(pathPoints);
        FindObjectOfType<PathCreator>().LineUpdate(pathPoints, lineRenderer);
    }
}
