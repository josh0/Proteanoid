using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
public abstract class EnemyPart : MonoBehaviour, ITargetable, IPointerEnterHandler, IPointerExitHandler
{
    public enum EnemyPartAttributes
    {
        /// <summary>This part will intercept attacks against other parts.</summary>
        blocking,
        /// <summary>This part will damage the enemy directly instead of taking damage and breaking.</summary>
        vital,
        /// <summary>This part can be regenerated using the Regenerate action.</summary>
        regenerative
    }
    public List<StatusEffect> effects = new();
    public List<EnemyPartAttributes> attributes = new();
    public List<ActionConstructor> addedActions;
    [field: SerializeField] public int hp { get; private set; }
    public bool isPartBroken;

    public Enemy parentEnemy;

    private Button button;

    private bool isMouseOverPart;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public virtual int OnAttack(int amount)
    {
        return parentEnemy.OnAttackPart(this, amount, true);
    }

    /// <summary>
    /// Directly damages the given part.
    /// </summary>
    /// <param name="amount">The amount of damage to deal.</param>
    /// <param name="procsOnHitEffects">Whether or not this damage should proc on-hit effects (was it caused by an attack?)</param>
    /// <returns></returns>
    public virtual int TakeDamage(int amount, bool procsOnHitEffects)
    {
        hp -= amount;

        if (hp <= 0)
            BreakPart();

        return amount;
    }

    public virtual void BreakPart()
    {
        isPartBroken = true;
        button.interactable = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        //This is checked manually instead of through events in case of clicking and dragging.
        if (button.interactable && isMouseOverPart && Input.GetMouseButtonUp(0))
            TargetSelector.Instance.SelectTarget(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverPart = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverPart = false;
    }

    #region Targetable Interface Implementation

    public void AddBlock(int amount)
    {
        parentEnemy.AddBlock(amount);
    }

    public void AddEffect(StatusEffect effect, int stacks)
    {
        parentEnemy.AddEffect(effect, stacks);
    }

    public void RemoveEffect<T>() where T : StatusEffect
    {
        parentEnemy.RemoveEffect<T>();
    }
    #endregion
}
