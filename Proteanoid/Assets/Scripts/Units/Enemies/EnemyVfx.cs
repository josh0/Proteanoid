using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Controls button and canvas data for the enemy. Should be attached to the enemy's graphic.
/// </summary>

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ShakeMovement))]
public class EnemyVfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOverEnemy;
    [SerializeField] private Enemy baseEnemyClass;
    private Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(baseEnemyClass.enemyName);
        Tooltip.Instance.SetUnitTooltip(transform, new List<UnitAction> { baseEnemyClass.intent }, baseEnemyClass.enemyName, baseEnemyClass.statusEffects);
        isMouseOverEnemy = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverEnemy = false;
        Tooltip.Instance.ClearTooltip(transform);
    }

    private void Update()
    {
        if (isMouseOverEnemy && Input.GetMouseButtonUp(0))
            TargetSelector.Instance.SelectTarget(baseEnemyClass);
    }
}
