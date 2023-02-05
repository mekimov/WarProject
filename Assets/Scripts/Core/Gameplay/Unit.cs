using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class Unit : MonoBehaviour
{
    public PathMover pathmover;
    public Animator animator;
    public Stats stats;
    public GameObject selectionAura;
    public GameObject healthBar;
    public Unit attackTarget;
    public Vector3 retreatPoint = new Vector3();
    public Player owner;

    [SerializeField] public Button _beginFightButton;
    [SerializeField] public NavMeshAgent navMeshAgent;

    public float MaxWayPerTurn()
    {
        return (stats.stamina);
    }

    public void DoAttack(Unit c)
    {
        transform.LookAt(c.transform);

        c.ReceiveDamage(stats.Attack);
        animator.SetTrigger("Attack");
    }

    public void DoRetreat(Unit c)
    {
        navMeshAgent.SetDestination(retreatPoint);
        transform.LookAt(c.transform);

    }

    public void ReceiveDamage(int damage)
    {
        stats.CurrentHP -= damage;
        animator.SetTrigger("ReceiveDamage");
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
        Game.Instance.BattleController.ActivePlayer.TrySelectUnit(this);
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
        UnitDead();

    }
    public bool MoveFinished()
    {
        return (pathmover.MoveFinished());
    }

    public void UnitDead()
    {
        if (stats.CurrentHP <= 0)
        {
            animator.SetTrigger("Die");
            selectionAura.SetActive(false);
            owner.AllUnits.Remove(this);
            var colliders = GetComponentsInChildren<Collider>(); //<> - дженерик, подставной шаблон
            foreach (var c in colliders)
            {
                c.enabled = false;
            }
        }
        return;
    }
  
}
