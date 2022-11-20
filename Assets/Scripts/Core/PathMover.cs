using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PathMover : MonoBehaviour
{
    public static PathMover Instance { get; private set; }
    [SerializeField] private NavMeshAgent navmeshagent;
    [SerializeField] private Animator animator;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    Vector3 prevPosition;

    public delegate void MethodContainer(Queue<Vector3> pathUpdate);
    public event MethodContainer onLineDeque;

    //public Action<Queue> actionRedrawLine;
    
    public bool waitForCommand;

    public void LogPointsInfo()
    {
        Debug.LogError(pathPoints.Count);
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

    private void SetPoints(IEnumerable<Vector3> points)
    {
        if (pathPoints.Count > 0 && points.Count() == 0)
            return;
        //Debug.LogError("SetPoints" + points);
        pathPoints = new Queue<Vector3>(points); 
        //if (pathPoints.Count > 0)
            //waitForCommand = false;
    }

    public void Start()
    {
        Instance = this;
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
            onLineDeque?.Invoke(pathPoints);//Здесь нужно обновить PathCreator
        }
    }

    private bool ShouldSetDestination()
    {
        Debug.Log("pathPoints.Count = " + pathPoints.Count);
        if (pathPoints.Count == 0)
            return false;
        if (waitForCommand)
            return false;
        if (navmeshagent.hasPath == false || navmeshagent.remainingDistance < 1f)
            return true;
        return false;
    }

    public bool TurnFinished()
    {
        return (pathPoints.Count == 0 && !waitForCommand);
    }
}
