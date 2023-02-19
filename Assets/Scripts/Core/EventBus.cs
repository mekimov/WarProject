using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public event Action<Unit> onUnitKilled;
    public event Action onPlayerWin;
    public event Action onPlayerLoose;

    public void OnUnitKilled(Unit unit)
    {
        if (onUnitKilled != null)
        {
            onUnitKilled.Invoke(unit);
        }
    }

    public void OnPlayerWin()
    {
        onPlayerWin?.Invoke();
    }

    public void OnPlayerLoose()
    {
        onPlayerLoose?.Invoke();
    }
}