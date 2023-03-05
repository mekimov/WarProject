using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Serialization;

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

    [SerializeField]
    private UnitState _unitState = UnitState.Wait;
    public UnitState UnitState => _unitState;

    public void SetPhase(UnitState phase)
    {
        _unitState = phase;
    }
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

    public void SetPoints(IEnumerable<Vector3> points)
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
            Vector3 nextPoint = pathPoints.Dequeue(); //достаем последний элемент из очереди
           
            navmeshagent.SetDestination(nextPoint);
            onLineDeque?.Invoke(pathPoints);
            FindObjectOfType<PathCreator>().LineUpdate(pathPoints, lineRenderer);
        }
    }

    private bool ShouldSetDestination()
    {

        if (pathPoints.Count == 0)
            return false;
        if (_unitState != UnitState.Acting)
            return false;
        if (navmeshagent.hasPath == false || navmeshagent.remainingDistance < 1f)
            return true;
        return false;

    }

    public bool MoveFinished()
    {
        return (pathPoints.Count == 0 && _unitState != UnitState.Prepare);
    }

    public void StopMoving()
    {
        pathPoints.Clear();
        _unitState = UnitState.Wait;
        onLineDeque?.Invoke(pathPoints);
        FindObjectOfType<PathCreator>().LineUpdate(pathPoints, lineRenderer);
    }
}

public enum UnitState
{
    Prepare = 1,
    Acting = 2,
    Wait = 3,
}