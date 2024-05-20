using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
public abstract class EnemyPart : MonoBehaviour, ITargetable, IPointerEnterHandler, IPointerExitHandler
{
    public List<StatusEffect> effects = new List<StatusEffect>();
    public int hp { get; private set; }
    public bool isPartBroken;

    public Enemy parentEnemy;

    private Button button;

    private bool isMouseOverPart;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public virtual int TakeDamage(int amount, bool procsOnHitEffects)
    {
        hp -= amount;

        if (hp <= 0)
            BreakPart();

        return parentEnemy.TakeDamage(amount, procsOnHitEffects);
    }

    public virtual void BreakPart()
    {
        button.interactable = false;
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
        throw new System.NotImplementedException();
    }

    public void RemoveEffect<T>() where T : StatusEffect
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
