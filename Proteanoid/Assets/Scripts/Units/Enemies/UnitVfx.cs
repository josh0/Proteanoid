using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Controls button and canvas data, among other misc visual effects. <br />
/// Should be attached to the enemy's graphic renderer.
/// </summary>

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ShakeMovement))]
public class UnitVfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOverUnit;
    public Unit baseUnitClass;
    [SerializeField] private ShakeMovement graphicShaker;
    private Image graphicRenderer;

    /// <summary>The script controlling this unit's movement.</summary>
    public UnitMovement movement { get; private set; }

    [field: SerializeField] public HPSlider hpSlider { get; private set; }
    [SerializeField] private ActionDescription intentDescription;

    public void UpdateIntentIcon(UnitAction intent)
    {
        intentDescription.SetDescription(intent);
    }

    private void Awake()
    {
        graphicRenderer = GetComponent<Image>();
    }

    private void Start()
    {
        hpSlider.SetMaxHPVal(baseUnitClass.maxHp);
        hpSlider.SetHPVal(baseUnitClass.hp);
        hpSlider.SetBlockVal(baseUnitClass.block);
    }
    public void TakeDamage()
    {
        StartCoroutine(graphicShaker.ShakeForDuration(0.2f));
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        graphicRenderer.color = new Color(1f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        graphicRenderer.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (baseUnitClass is Enemy enemy)
            Tooltip.Instance.SetUnitTooltip(transform, new List<UnitAction> { enemy.intent }, baseUnitClass);
        else
            Tooltip.Instance.SetUnitTooltip(transform, new List<UnitAction> { }, baseUnitClass);
        isMouseOverUnit = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverUnit = false;
        Tooltip.Instance.ClearTooltip(transform);
    }

    
}
