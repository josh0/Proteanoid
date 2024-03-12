using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionDescription : MonoBehaviour
{
    [SerializeField] private Image actionImage;
    [SerializeField] private TextMeshProUGUI actionPowerText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI nameText;

    /// <summary>
    /// Set the gameObject to active, set the description icon to the given action's icon, and set the power text to the action's power.
    /// </summary>
    public void SetDescription(UnitAction action)
    {
        SetActive(true);
        actionImage.sprite = action.icon;
        if (actionPowerText != null)
            actionPowerText.text = action.power.ToString();
    }

    /// <summary>
    /// Same as first override, but displays a status effect instead of an action.
    /// </summary>
    public void SetDescription(StatusEffect effect)
    {
        SetActive(true);
        actionImage.sprite = effect.icon;
        if (actionPowerText != null)
            actionPowerText.text = effect.stacks.ToString();
    }

    /// <summary>
    /// SetDescription(action), then also enable the description text (if this object has one), and update it. <br />
    /// Should only be used for the Tooltip Box.
    /// </summary>
    public void SetDescriptionWithTooltip(UnitAction action)
    {
        SetDescription(action);
        if (descriptionText == null || nameText == null)
        {
            Debug.LogWarning("Tried to call SetDescriptionWithTooltip() on an action description with no description or name text.");
            return;
        }
        descriptionText.text = action.actionTooltip;
        nameText.text = action.actionName;
    }

    /// <summary>
    /// Same as first override, but displays a status effect instead of an action.
    /// </summary>
    public void SetDescriptionWithTooltip(StatusEffect effect)
    {
        SetDescription(effect);
        if (descriptionText == null || nameText == null)
        {
            Debug.LogWarning("Tried to call SetDescriptionWithTooltip() on an action description with no description or name text.");
            return;
        }
        descriptionText.text = effect.description;
        nameText.text = effect.name;
    }

    public void SetActive(bool a)
    {
        gameObject.SetActive(a);
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(a);
    }
}
