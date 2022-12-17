using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public PathMover pathmover;
    public Animator animator;
    public Stats stats;
    public GameObject selectionAura;
    [SerializeField] public Button _beginFightButton; 

    public float MaxWayPerTurn()
    {
        return (stats.stamina);
    }

    public void DoAttack(Character c)
    {
        c.ReceiveDamage(stats.Attack);
    }

    public void ReceiveDamage(int damage)
    {
        stats.CurrentHP -= damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Character>();
        if (target != null)
        {
            target.ReceiveDamage(stats.Attack);
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
        _beginFightButton.onClick.AddListener(OnUnselect);

    }
    public bool TurnFinished()
    {
        return (pathmover.TurnFinished());
    }
}
