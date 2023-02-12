using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    public event Action<Unit> onUnitKilled;

    public void OnUnitKilled(Unit unit)
    {
        if (onUnitKilled != null)
        {
            onUnitKilled.Invoke(unit);
        }
    }
}