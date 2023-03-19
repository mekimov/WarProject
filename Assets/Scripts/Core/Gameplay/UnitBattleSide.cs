using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public enum Side { Front = 1, Back = 2, Left = 3, Right = 4 };
public class UnitBattleSide : MonoBehaviour
{
    [SerializeField] Side _side;
    public Side Side => _side;
    [SerializeField] Unit _unit;
    public Unit Unit => _unit;

    public void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<UnitBattleSide>();
        if (target != null && _unit.owner != target.Unit.owner)
        {
            _unit.attackTarget = target.Unit;//DoAttack(target);
            _unit.pathmover.StopMoving();
            target.Unit.pathmover.StopMoving();
            // retreatPoint = -2f* (((transform.position + attackTarget.transform.position) / 2f) - transform.position);
            var deltaVector = -1 * (_unit.attackTarget.transform.position - transform.position).normalized;
            var retreatDistance = 10.0f;
            _unit.retreatPoint = transform.position + deltaVector * retreatDistance;
            _unit.stats.SetHitDirection(_side, target._side);
            _unit.attackTarget.stats.SetHitDirection(target._side, Side);
        }

    }

}
