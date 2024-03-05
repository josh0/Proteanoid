using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Tooltip : Singleton<Tooltip>
{
    float targetAlpha = 0;
    private CanvasGroup canvasGroup;

    private Transform activeTooltipSender;
    private RectTransform rectTransform;

    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private float fadeSpeed;

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
    /// If the 
    /// </summary>
    /// <param name="sender">The MonoBehaviour send</param>
    /// <param name="tooltip">The string to be displayed in the tooltip box.</param>
    /// <param name="yValueOffset"></param>
    public void SetTooltip(Transform sender, string tooltip, float yValueOffset)
    {
        if (tooltip == null && sender == activeTooltipSender)
            ClearTooltip();
        else if (tooltip == null && sender != activeTooltipSender)
            return;
        else
        {
            activeTooltipSender = sender;
            SetPosNextToTransform(sender, yValueOffset);
            descText.text = tooltip;
            targetAlpha = 1;
        }
    }

    private void SetPosNextToTransform(Transform t, float yValueOffset)
    {
        if (t.position.y > 0)
            transform.position = t.position + Vector3.up * (yValueOffset + rectTransform.sizeDelta.y);
        else
            transform.position = t.position - Vector3.up * (yValueOffset + rectTransform.sizeDelta.y);
        targetAlpha = 1;
    }

    private void ClearTooltip()
    {
        targetAlpha = 0;
    }
}
