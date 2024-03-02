using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Controls button and canvas data for the enemy. Should be attached to the enemy's graphic.
/// </summary>

[RequireComponent(typeof(Button))]
public class EnemyVfx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Enemy baseEnemyClass;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        TargetSelector.Instance.SelectTarget(baseEnemyClass);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(baseEnemyClass.enemyName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
