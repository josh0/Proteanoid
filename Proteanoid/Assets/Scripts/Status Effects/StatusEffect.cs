using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be added to a unit, and to be activated at various points in a round.
/// </summary>
public abstract class StatusEffect : ScriptableObject
{
    public int stacks;

    public Sprite icon;
    public string effectName;
    public string description;

    /// <summary>
    /// Removes a given number of stacks. If stacks are 0 or less, remove this status effect.
    /// </summary>
    /// <param name="amount">The amount of stacks to remove.</param>
    public void RemoveStacks(int amount)
    {
        stacks -= amount;
        if (amount <= 0) Destroy(this);
    }

    public virtual void OnRoundEnd(Unit affectedUnit) { }
    public virtual void OnTakeDamage(Unit affectedUnit) { }
    public virtual void OnStartTurn(Unit affectedUnit) { }
}
