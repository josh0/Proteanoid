using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardButton))]
public class CardButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int originalSiblingIndex;
    [SerializeField] private Transform targetTransform;
    private Vector3 moveOffset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float downwardOffsetWhenInactive;

    private CardButton baseCardButtonClass;

    private void Awake()
    {
        originalSiblingIndex = transform.GetSiblingIndex();
        moveOffset = Vector3.down * downwardOffsetWhenInactive;
        baseCardButtonClass = GetComponent<CardButton>();
    }
    public Vector3 targetPos;
    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (CardManager.Instance.heldCardButton != baseCardButtonClass)
            targetPos = targetTransform.position + moveOffset;
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    public void SetTargetTransformActive(bool active)
    {
        if (targetTransform != null)
            targetTransform.gameObject.SetActive(active);
    }

    public void SetTargetTransform(Transform t) =>
        targetTransform = t;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        Tooltip.Instance.SetActionTooltip(transform, baseCardButtonClass.heldCard.actions, GetStatusEffectsToDisplay());
        moveOffset = Vector3.zero;
    }

    /// <returns>All effects applied by all actions in the held card. <br />
    /// If there are no applied effects, instead returns null</returns>

    private List<StatusEffect> GetStatusEffectsToDisplay()
    {
        List<StatusEffect> totalEffects = new();
        foreach(UnitAction action in baseCardButtonClass.heldCard.actions)
        {
            if (action.appliedEffect != null)
                totalEffects.Add(action.appliedEffect);
        }
        if (totalEffects.Count > 0)
            return totalEffects;
        else return null;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(originalSiblingIndex);
        moveOffset = Vector3.down * downwardOffsetWhenInactive;
        Tooltip.Instance.ClearTooltip(transform);
    }

}
