using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information about an action that can be assigned in the inspector.
/// </summary>
[Serializable]
public class ActionConstructor
{
    public UnitAction action;
    public int actionPower;
    public StatusEffect effect;
    public int effectStacks;

    /// <summary>Instantiates a new action with the stats provided in the constructor.</summary>
    /// <returns></returns>
    public UnitAction CreateAction()
    {
        UnitAction a = UnityEngine.Object.Instantiate(action);
        a.power = actionPower;
        a.appliedEffect = effect;
        a.appliedEffectStacks = effectStacks;
        return a;
    }
}
