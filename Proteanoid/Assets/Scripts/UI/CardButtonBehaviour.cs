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
    private void Update()
    {
        Vector3 targetPos = targetTransform.position + moveOffset;
        transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    public void DisableTargetTransform()
    {
        if (targetTransform != null)
            targetTransform.gameObject.SetActive(false);
    }

    public void SetTargetTransform(Transform t) =>
        targetTransform = t;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        Tooltip.Instance.SetTooltip(transform, baseCardButtonClass.heldCard.actions, 3);
        moveOffset = Vector3.zero;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(originalSiblingIndex);
        moveOffset = Vector3.down * downwardOffsetWhenInactive;
        Tooltip.Instance.ClearTooltip(transform);
    }

}
