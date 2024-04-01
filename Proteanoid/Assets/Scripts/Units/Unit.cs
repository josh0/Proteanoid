using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Any unit in the fight, including the player and enemies.
/// </summary>
public abstract class Unit : ScriptableObject
{
    public string unitName;
    [HideInInspector] public int hp;
    public int maxHp;
    public int strength;
    public Sprite sprite;

    [HideInInspector] public UnitLoader loader;

    /// <summary>The list of status effects that should be displayed in the tooltip box.</summary>
    [HideInInspector] public List<StatusEffect> statusEffects = new();

    public int block;

    //Both of these next methods make a copy of the statusEffect list to avoid the collection modified error.

    public virtual void OnStartTurn()
    {
        foreach (StatusEffect effect in new List<StatusEffect>(statusEffects))
            effect.OnStartTurn(this);
        block = 0;
    }

    public void OnRoundEnd()
    {
        foreach (StatusEffect effect in new List<StatusEffect>(statusEffects))
            effect.OnRoundEnd(this);
    }

    public abstract IEnumerator TurnRoutine();

    /// <summary>
    /// Deals damage adjusted for status effects.
    /// </summary>
    /// <param name="amount">The base amount of damage the unit should take before calculations.</param>
    /// <param name="procsOnDamageEffects">Whether or not damage dealt this way should call the OnTakeDamage event.</param>
    /// <returns>The amount of unblocked damage dealt.</returns>
    public int TakeDamage(int amount, bool procsOnDamageEffects)
    {
        int finalDamage = amount;

        if (block >= amount)
        {
            AddBlock(-amount);
            amount = 0;
        }
        else
        {
            amount -= block;
            AddBlock(-block);
        }

        if (amount > 0)
        {
            hp -= amount;

            if (procsOnDamageEffects)
            {
                //Make a copy of the list to avoid the collection modified error
                foreach (StatusEffect effect in new List<StatusEffect>(statusEffects))
                    effect.OnTakeDamage(this);
            }
        }

        loader.hpSlider.SetHPVal(hp);

        if (hp <= 0)
            Die();

        return finalDamage;
    }

    protected abstract void Die();

    public void AddBlock(int amount)
    {
        block = Mathf.Max(0, block + amount);
        loader.hpSlider.SetBlockVal(block);
    }

    /// <summary>
    /// Adds any status effect script to the unit, or, if it already has that effect, increase the effect's stacks.
    /// </summary>
    /// <param name="effect">The effect to add</param>
    public void AddEffect(StatusEffect effect, int stacks)
    {
        foreach (StatusEffect existingEffect in statusEffects)
        {
            if (existingEffect.GetType() == effect.GetType())
            {
                existingEffect.AddStacks(stacks);
                loader.hpSlider.SetStatusEffectDescriptions(statusEffects);
                return;
            }
        }
        StatusEffect newEffect = ScriptableObject.Instantiate(effect);
        newEffect.AddStacks(stacks);
        statusEffects.Add(newEffect);
        
        loader.hpSlider.SetStatusEffectDescriptions(statusEffects);
    }

    /// <summary>Removes a given type of effect.</summary>
    /// <typeparam name="T">The type of effect to remove.</typeparam>
    public void RemoveEffect<T>() where T : StatusEffect =>
        RemoveEffect(statusEffects.OfType<T>().First());

    /// <summary>Removes a given effect and updates the HP Slider effect display.</summary>
    /// <param name="effect">The effect to remove</param>
    public void RemoveEffect(StatusEffect effect)
    {
        statusEffects.Remove(effect);
        loader.hpSlider.SetStatusEffectDescriptions(statusEffects);
    }

    public void RemoveEffectStacks<T>(int stacks) where T : StatusEffect
    {
        StatusEffect effect = statusEffects.OfType<T>()?.First();
        if (effect == null)
            return;
        effect.AddStacks(-stacks);
        if (effect.stacks <= 0)
            RemoveEffect(effect);
        loader.hpSlider.SetStatusEffectDescriptions(statusEffects);
    }
}
