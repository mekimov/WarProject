using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField] List<Unit> units; //список всех персонажей игрока
    public List<Unit> AllUnits => units;
    [SerializeField] Unit selectedUnit;

    [SerializeField] private Color _color;
    public Color Color => _color;

    public Unit SelectedUnit => selectedUnit;
    public bool isFirstPlayer = false;
    private bool isActivePlayer = false;

    private void Start()
    {
        foreach (var unit in units)
        {
            unit.owner = this; //this -- обращение к текущему экземпляру класса
        }
    }

    private void Update()
    {
        if (isActivePlayer)
            ProcessInput();
    }

    protected abstract void ProcessInput();
    protected virtual void ProcessBeginTurn() { }
    protected virtual void ProcessEndTurn() { }




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
        isActivePlayer = true;
        ProcessBeginTurn();
    }
    public void OnEndTurn()
    {
        foreach (var unit in AllUnits)
        {
            unit.OnUnselect();
        }
        OnUnitUnselected();
        isActivePlayer = false;
        ProcessEndTurn();

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

