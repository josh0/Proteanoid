using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be added to a unit, and to be activated at various points in a round.
/// </summary>
public abstract class StatusEffect : MonoBehaviour
{
    protected Unit affectedUnit;
    public int stacks;

    public void SetAffectedUnit(Unit unit)
    {
        affectedUnit = unit;
        affectedUnit.OnTakeDamage += OnTakeDamage;
        affectedUnit.OnStartTurn += OnStartTurn;
    }

    protected virtual void OnEnable()
    {
        FightManager.OnRoundEnd += OnRoundEnd;
        FightManager.OnRoundStart += OnRoundStart;
    }

    protected virtual void OnDisable()
    {
        if (affectedUnit == null)
            return;
        FightManager.OnRoundStart -= OnRoundStart;
        FightManager.OnRoundEnd -= OnRoundEnd;
        affectedUnit.OnTakeDamage -= OnTakeDamage;
        affectedUnit.OnStartTurn -= OnStartTurn;
    }

    /// <summary>
    /// Removes a given number of stacks. If stacks are 0 or less, remove this status effect.
    /// </summary>
    /// <param name="amount">The amount of stacks to remove.</param>
    public void RemoveStacks(int amount)
    {
        stacks -= amount;
        if (amount <= 0) Destroy(this);
    }

    protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnTakeDamage() { }
    protected virtual void OnStartTurn() { }
}
