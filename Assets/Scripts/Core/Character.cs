using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public PathMover pathmover;
    public Animator animator;
    public int stamina = 50;
    public GameObject selectionAura;

    public float MaxWayPerTurn()
    {
        return (stamina);
    }

    public void OnSelect()
    {
        pathmover.OnSelect();
        selectionAura.SetActive(true);


    }
    public void OnUnselect()
    {
        pathmover.OnUnselect();
        selectionAura.SetActive(false);
    }

    public void OnBeginTurn()
    {
        pathmover.waitForCommand = true;
    }

    public void OnMouseDown()
    {
        BattleController.Instance.ActivePlayer.TrySelectCharacter(this);
    }
    private void Awake()
    {
        selectionAura.SetActive(false);
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
