using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any action that a unit might perform. Cards are lists of actions.
/// </summary>
public abstract class UnitAction : ScriptableObject
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

    /// <summary>The effect applied by this action.</summary>
    public StatusEffect appliedEffect;
    /// <summary>The amount of stacks the appliedEffect should apply.</summary>
    public int appliedEffectStacks;

    public string actionName;
    [Tooltip("Some tags: [power] [stacks]")]
    [SerializeField] private string actionTooltip;

    [HideInInspector] public int damageModifier = 0;
    [HideInInspector] public int blockModifier = 0;

    /// <summary>
    /// What the unit will do when this action is called.
    /// </summary>
    /// <param name="targets">The target(s) who this action will affect.</param>
    public abstract IEnumerator OnAct(Unit actor, Unit targets);

    protected void ApplyEffectToTarget(Unit target)
    {
        if (appliedEffect != null && appliedEffectStacks != 0)
        target.AddEffect(appliedEffect, appliedEffectStacks);
    }

    public string GetTooltip(Unit actor)
    {
        String s = actionTooltip.Replace("[power]", GetPredictedPower(actor).ToString());

        if (appliedEffect != null)
        {
            if (targetType == TargetType.self)
                s += "and gain ";
            else
                s += "and apply ";
            s += appliedEffectStacks + " " + appliedEffect.effectName;
        }

        return s;
    }

    protected int GetPredictedPower(Unit actor)
    {
        if (this is AttackAction)
            return power + damageModifier + actor.strength;
        else if (this is BlockAction)
            return power + blockModifier;
        else
            return power;
    }
}
