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

    public void OnStartTurn()
    {
        foreach (StatusEffect effect in statusEffects)
            effect.OnStartTurn(this);
    }

    public void OnRoundEnd()
    {
        foreach (StatusEffect effect in statusEffects)
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
                foreach (StatusEffect effect in statusEffects)
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
    /// <typeparam name="T">Any status effect (eg CutEffect).</typeparam>
    /// <param name="stacks">The amount of stacks to add to the effect.</param>
    public void AddEffect(StatusEffect effect)
    {
        foreach (StatusEffect existingEffect in statusEffects)
        {
            if (existingEffect.GetType() == effect.GetType())
            {
                existingEffect.stacks += effect.stacks;
                return;
            }
        }
        statusEffects.Add(effect);
    }

    public void RemoveEffect(StatusEffect effect)
    {
        statusEffects.Remove(effect);
        UpdateEffectDisplays();
    }

    private void UpdateEffectDisplays()
    {

    }
}
