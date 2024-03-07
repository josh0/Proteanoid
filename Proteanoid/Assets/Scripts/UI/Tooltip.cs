using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Tooltip : Singleton<Tooltip>
{
    float targetAlpha = 0;
    private CanvasGroup canvasGroup;

    private Transform activeTooltipSender;
    private RectTransform rectTransform;

    [SerializeField] private float fadeSpeed;

    [SerializeField] private List<ActionDescription> actionDescriptions;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Display the description of each unique action in a given list of actions in the tooltip box. <br />
    /// If more than 5 unique actions are provided, only the first 5 will be displayed.
    /// </summary>
    public void SetTooltip(Transform sender, List<UnitAction> actionsToDisplay, float xValueOffset)
    {
        List<UnitAction> actions = actionsToDisplay.Distinct().ToList();
        activeTooltipSender = sender;
        for(int i = 0; i < actionDescriptions.Count; i++)
        {
            if (actions.Count >= i + 1)
                actionDescriptions[i].SetDescriptionWithTooltip(actions[i]);
            else
                actionDescriptions[i].SetActive(false);
        }

        SetPosNextToTransform(sender, xValueOffset);
    }

    public void ClearTooltip(Transform sender)
    {
        if (sender == activeTooltipSender)
            targetAlpha = 0;
    }

    /// <summary>
    /// Sets the tooltip box next to a given transform with a given x offset. <br />
    /// Should be used AFTER SetTooltip().
    /// </summary>
    /// <param name="t">The transform the tooltip box should appear next to.</param>
    /// <param name="xValueOffset">The distance from the transform the tooltip box should appear at on the x value.</param>
    private void SetPosNextToTransform(Transform t, float xValueOffset)
    {
        if (t.position.x > 0)
            xValueOffset = Mathf.Abs(xValueOffset) * -1;
        else
            xValueOffset = Mathf.Abs(xValueOffset);

        transform.position = t.position + Vector3.right * xValueOffset;
        targetAlpha = 1;
        //PlaceTooltipWithinScreen();
    }

    private void PlaceTooltipWithinScreen()
    {
        Vector2 wScreenPos = Camera.main.WorldToScreenPoint(rectTransform.position);
        Debug.Log("*** Word Screen Position: " + Camera.main.WorldToScreenPoint(rectTransform.position));

        wScreenPos.x = wScreenPos.x + ((rectTransform.rect.width / 2f) - wScreenPos.x);
        Debug.Log("*** word new Screen pos: " + wScreenPos);
        Vector3 outV = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, wScreenPos, Camera.current, out outV);
        //            Vector2 wWorldPos = Camera.main.ScreenToWorldPoint(wScreenPos);
        Vector2 wWorldPos = (Vector2)outV;
        rectTransform.position = wWorldPos;
    }
}
