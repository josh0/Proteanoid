using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
public abstract class EnemyPart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public virtual void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
            BreakPart();
    }

    public virtual void BreakPart()
    {

    }

    private void Update()
    {
        //This is checked manually instead of through events in case of clicking and dragging.
        if (button.interactable && isMouseOverPart && Input.GetMouseButtonUp(0))
            TargetSelector.Instance.SelectTarget(parentEnemy);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverPart = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverPart = false;
    }
}
