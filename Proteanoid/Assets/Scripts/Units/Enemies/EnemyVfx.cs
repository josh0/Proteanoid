using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Controls button and canvas data for the enemy. Should be attached to the enemy's graphic.
/// </summary>

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ShakeMovement))]
public class EnemyVfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private Enemy baseEnemyClass;
    private Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(baseEnemyClass.enemyName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        TargetSelector.Instance.SelectTarget(baseEnemyClass);
    }
}
