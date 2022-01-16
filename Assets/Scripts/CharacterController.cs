using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {
        animator.SetTrigger("Shoot");
    }
}
