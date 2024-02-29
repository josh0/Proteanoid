using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float scaleOffset = 0;
    [SerializeField] private float scaleIncreaseOnHover;
    [SerializeField] private float scaleSpeed;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        scaleOffset = 0;
    }
    private void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one + (Vector2.one * scaleOffset), Time.deltaTime * scaleSpeed);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            scaleOffset = scaleIncreaseOnHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleOffset = 0;
    }
}
