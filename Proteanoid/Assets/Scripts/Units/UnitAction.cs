using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any action that a unit might perform. Cards are lists of actions.
/// </summary>
public abstract class UnitAction
{
    public enum TargetType
    {
        /// <summary>This action should target the caster.</summary>
        self,
        /// <summary>This action should target a chosen enemy; The player can choose one, enemies always target the player.</summary>
        enemy,
        /// <summary>Same as TargetType.enemy, but the player cannot choose a target.</summary>
        randomEnemy,
        /// <summary>Same as TargetType.enemy, but the player's cards target all enemies.</summary>
        allEnemies,
        /// <summary>Enemies only - Enemies target all their allies.</summary>
        allAllies
    }

    public TargetType targetType;

    /// <summary>The icon used to represent this action in enemy intents and on cards.</summary>
    public Sprite icon;
    /// <summary>The main number - How much damage it deals, how many stacks it gives, how much block it deals, etc etc.</summary>
    public int power;

    /// <summary>
    /// What the unit will do when this action is called.
    /// </summary>
    /// <param name="targets">The target(s) who this action will affect.</param>
    public abstract IEnumerator OnAct(Unit actor, List<Unit> targets);
}
