using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navmeshagent;
    [SerializeField] private Animator animator;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    Vector3 prevPosition;
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

    private void SetPoints(IEnumerable<Vector3> points)
    {
        pathPoints = new Queue<Vector3>(points);
        if (pathPoints.Count > 0)
        waitForCommand = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePathing();
        animator.SetFloat("MoveSpeed", (transform.position - prevPosition).magnitude / Time.deltaTime);
        prevPosition = transform.position;

    }

    private void UpdatePathing()
    {
        if (ShouldSetDestination())
            navmeshagent.SetDestination(pathPoints.Dequeue());
    }

    private bool ShouldSetDestination()
    {
        if (pathPoints.Count == 0)
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
