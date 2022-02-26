using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public PathMover pathmover;
    public Animator animator;

    public void OnSelect()
    {
        pathmover.OnSelect();

    }
    public void OnUnselect()
    {
        pathmover.OnUnselect();
    }

    public void OnBeginTurn()
    {
        pathmover.waitForCommand = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        animator.SetTrigger("Attack");
        
    }
    public bool TurnFinished()
    {
        return (pathmover.TurnFinished());
    }
}
