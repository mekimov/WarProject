using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] List<Unit> units; //список всех персонажей игрока
    public List<Unit> AllUnits => units;
    [SerializeField] Unit selectedUnit;

    [SerializeField] private Color _color;
    public Color Color => _color;

    public Unit SelectedUnit => selectedUnit;
    public bool isFirstPlayer = false;

    private void Start()
    {
        foreach (var unit in units)
        {
            unit.owner = this; //this -- обращение к текущему экземпляру класса
        }
    }
  
    void OnUnitSelected(Unit unitTarget)
    {
        if (selectedUnit != null)
            selectedUnit.OnUnselect();
        selectedUnit = unitTarget;
        selectedUnit.OnSelect();
    }
    void OnUnitUnselected()
    {
        if (selectedUnit != null)
            selectedUnit.OnUnselect();
    }

    public void TrySelectUnit(Unit target)
    {
        foreach (var c in units)
        {
            if (c == target)
               {
                OnUnitSelected(c);
                return;
            }
            
        }
    }
    public void OnBeginTurn()
    {
        OnUnitSelected(units[0]);
        foreach (var c in units)
        {
            c.OnBeginTurn();
        }
    }
    public void OnEndTurn()
    {
        OnUnitUnselected();
    }
    public bool AllUnitsFinishTurn()
    {
       foreach (var c in units)
        {
            if (!c.MoveFinished())
                return false;
        }
        return (true);
    }
}
