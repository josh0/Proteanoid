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
[RequireComponent(typeof(UnitMovement))]
public abstract class Unit : MonoBehaviour
{
    public int hp;
    [SerializeField] private int maxHp;
    [SerializeField] private ShakeMovement graphicShaker;
    [SerializeField] private Image graphicRenderer;

    /// <summary>The script controlling this unit's movement.</summary>
    public UnitMovement movement { get; private set; }

    ///<summary>The amount of damage added to this unit's attacks.</summary>
    public int strength;

    /// <summary>The list of status effects that should be displayed in the tooltip box.</summary>
    [HideInInspector] public List<StatusEffect> statusEffects = new();

    [SerializeField] private HPSlider hpSlider;

    private int block;

    protected virtual void Awake()
    {
        hp = maxHp;
        movement = GetComponent<UnitMovement>();
    }

    protected virtual void Start()
    {
        hpSlider.SetMaxHPVal(maxHp);
        hpSlider.SetHPVal(maxHp);
        hpSlider.SetBlockVal(0);
    }

    //Both of these next methods make a copy of the statusEffect list to avoid the collection modified error.

    public void OnStartTurn()
    {
        foreach (StatusEffect effect in new List<StatusEffect>(statusEffects))
            effect.OnStartTurn(this);
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
        if (gameObject == null)
            return 0;
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
                

            StartCoroutine(graphicShaker.ShakeForDuration(0.2f));
            StartCoroutine(FlashRed());
        }

        hpSlider.SetHPVal(hp);

        if (hp <= 0)
            Die();

        return finalDamage;
    }

    protected abstract void Die();

    private IEnumerator FlashRed()
    {
        graphicRenderer.color = new Color(1f,0.5f,0.5f);
        yield return new WaitForSeconds(0.1f);
        graphicRenderer.color = Color.white;
    }

    public void AddBlock(int amount)
    {
        block = Mathf.Max(0, block + amount);
        hpSlider.SetBlockVal(block);
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
                hpSlider.SetStatusEffectDescriptions(statusEffects);
                return;
            }
        }
        StatusEffect newEffect = ScriptableObject.Instantiate(effect);
        newEffect.AddStacks(stacks);
        statusEffects.Add(newEffect);
        
        hpSlider.SetStatusEffectDescriptions(statusEffects);
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
        hpSlider.SetStatusEffectDescriptions(statusEffects);
    }

    public void RemoveEffectStacks<T>(int stacks) where T : StatusEffect
    {
        StatusEffect effect = statusEffects.OfType<T>()?.First();
        if (effect == null)
            return;
        effect.AddStacks(-stacks);
        if (effect.stacks <= 0)
            RemoveEffect(effect);
        hpSlider.SetStatusEffectDescriptions(statusEffects);
    }
}
