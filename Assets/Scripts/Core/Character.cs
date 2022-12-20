using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public PathMover pathmover;
    public Animator animator;
    public Stats stats;
    public GameObject selectionAura;
    public Character attackTarget;
    public Vector3 retreatPoint = new Vector3();

    [SerializeField] public Button _beginFightButton;
    [SerializeField] public NavMeshAgent navMeshAgent;

    public float MaxWayPerTurn()
    {
        return (stats.stamina);
    }

    public void DoAttack(Character c)
    {
        c.ReceiveDamage(stats.Attack);
        animator.SetTrigger("Attack");
    }

    public void DoRetreat(Character c)
    {
        navMeshAgent.SetDestination(retreatPoint); // - обратиться к навмешу кэрэктера, думаю, метод надо писать там
    }

    public void ReceiveDamage(int damage)
    {
        stats.CurrentHP -= damage;
        animator.SetTrigger("ReceiveDamage");
    }

    public void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Character>();
        if (target != null)
        {
            attackTarget = target;//DoAttack(target);
            pathmover.StopMoving();
            retreatPoint = -2f* (((transform.position + attackTarget.transform.position) / 2f) - transform.position);
        }

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
        attackTarget = null;
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
            _beginFightButton.onClick.AddListener(OnUnselect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("Attack");

    }
    public bool MoveFinished()
    {
        return (pathmover.MoveFinished());
    }
  
}
