using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Tooltip : Singleton<Tooltip>
{
    float targetAlpha = 0;
    private CanvasGroup canvasGroup;

    private Transform activeTooltipSender;
    private RectTransform rectTransform;

    [SerializeField] private float fadeSpeed;

    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private List<ActionDescription> actionDescriptions;
    [SerializeField] private List<ActionDescription> statusDescriptions;

    [SerializeField] private RectTransform statusTooltipRect;

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
    public void SetActionTooltip(Transform sender, List<UnitAction> actionsToDisplay, List<StatusEffect> statusEffectsToDisplay)
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

        //Display status effects, if there are any. Otherwise, disable the status tooltip box.
        if (statusEffectsToDisplay == null || statusEffectsToDisplay.Count == 0)
            statusTooltipRect.gameObject.SetActive(false);
        else
            SetStatusDescriptions(statusEffectsToDisplay.Distinct().ToList());

        SetPosNextToTransform(sender);
    }

    private void SetStatusDescriptions(List<StatusEffect> effects)
    {
        statusTooltipRect.gameObject.SetActive(true);
        for (int i = 0; i < statusDescriptions.Count; i++)
        {
            if (effects.Count >= i + 1)
                statusDescriptions[i].SetDescriptionWithTooltip(effects[i]);
            else
                statusDescriptions[i].SetActive(false);
        }
    }

    /// <summary>
    /// Same as SetActionTooltip, but meant for units - Displays actions, a name, and Status effects, if the unit has any status effects.
    /// </summary>
    /// <param name="sender">The transform sending this tooltip.</param>
    /// <param name="actionsToDisplay">The actions that should be displayed.</param>
    /// <param name="label">The name of the unit sending this tooltip.</param>
    /// <param name="statusEffectsToDisplay">The status effects to display in the tooltip box - If null, disable the tooltip box.</param>
    public void SetUnitTooltip(Transform sender, List<UnitAction> actionsToDisplay, string label, List<StatusEffect> statusEffectsToDisplay)
    {
        SetActionTooltip(sender, actionsToDisplay, statusEffectsToDisplay);
        labelText.gameObject.SetActive(true);
        labelText.text = label;
    }

    public void ClearTooltip(Transform sender)
    {
        if (sender == activeTooltipSender)
            targetAlpha = 0;
    }

    /// <summary>
    /// Sets the tooltip box next to a given transform with a given x offset. <br />
    /// Should be used AFTER SetActionTooltip().
    /// </summary>
    /// <param name="t">The transform the tooltip box should appear next to.</param>
    private void SetPosNextToTransform(Transform t)
    {
        float xValueOffset = 3;

        if (t.position.x > 0)
            xValueOffset = Mathf.Abs(xValueOffset) * -1;
        else
            xValueOffset = Mathf.Abs(xValueOffset);

        transform.position = t.position + Vector3.right * xValueOffset;
        statusTooltipRect.localPosition = Vector3.right * statusTooltipRect.rect.width * (xValueOffset / Mathf.Abs(xValueOffset));
        targetAlpha = 1;
        StartCoroutine(ClampRectTransformRoutine(rectTransform));
        StartCoroutine(ClampRectTransformRoutine(statusTooltipRect));
    }

    /// <summary>
    /// Waits one frame, then clamps a given RectTransform to always be entirely visible on-screen.
    /// </summary>
    /// <param name="t">The RectTransform to clamp.</param>
    private IEnumerator ClampRectTransformRoutine(RectTransform t)
    {
        yield return null;
        Camera camera = Camera.main;
        Vector2 bottomLeft = camera.ScreenToWorldPoint(Vector3.zero);
        Vector2 topRight = camera.ScreenToWorldPoint(new Vector3(
            camera.pixelWidth, camera.pixelHeight));

        var cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);

        Vector2 tSize = t.TransformVector(t.sizeDelta);

        t.position = new Vector3(
            Mathf.Clamp(t.position.x, cameraRect.xMin + (tSize.x / 2), cameraRect.xMax - (tSize.x / 2)),
            Mathf.Clamp(t.position.y, cameraRect.yMin + (tSize.y / 2), cameraRect.yMax - (tSize.y / 2)),
            t.position.z);
    }
}
