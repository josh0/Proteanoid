using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be added to a unit, and to be activated at various points in a round.
/// </summary>
public abstract class StatusEffect : ScriptableObject
{
    public int stacks { get; private set; }

    public Sprite icon;
    [Tooltip("The name to be displayed in the tooltip (eg Poisoned, Weak, etc)")]
    public string effectName;

    [Tooltip("The adjective to be used for card names (eg Venomous, Hearty, etc)")]
    public string adjective;

    [Tooltip("[stacks] will be replaced accordingly.")]
    [SerializeField] private string description;

    /// <summary>
    /// Adds a given number of stacks and updates the stack counter. <br />
    /// If stacks are 0 or less, remove this status effect. <br />
    /// </summary>
    /// <param name="amount">The amount of stacks to remove.</param>
    public void AddStacks(int amount)
    {
        stacks += amount;
    }

    public virtual void OnRoundEnd(Unit affectedUnit) { }
    public virtual void OnTakeDamage(Unit affectedUnit) { }
    public virtual void OnStartTurn(Unit affectedUnit) { }

    public string GetTooltip(bool showStacks)
    {
        if (showStacks)
            return description.Replace("[stacks]", stacks.ToString());
        else
            return description.Replace("[stacks]", "X");
    }
}
