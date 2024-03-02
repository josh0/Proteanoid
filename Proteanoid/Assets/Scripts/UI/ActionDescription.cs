using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionDescription : MonoBehaviour
{
    [SerializeField] private Image actionImage;
    [SerializeField] private TextMeshProUGUI actionPowerText;

    /// <summary>
    /// Set the gameObject to active, set the description icon to the given action's icon, and set the power text to the action's power.
    /// </summary>
    public void SetDescription(UnitAction action)
    {
        gameObject.SetActive(true);
        actionImage.sprite = action.icon;
        actionPowerText.text = action.power.ToString();
    }
}
