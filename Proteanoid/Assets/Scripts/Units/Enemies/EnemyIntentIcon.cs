using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VolumeComponent;

/// <summary>
/// Controls the SpriteRenderer, TextMeshPro, and ShakeMovement of an enemy's intent icon.
/// </summary>

public class EnemyIntentIcon : MonoBehaviour
{
    [SerializeField] private ShakeMovement intentShaker;

    [SerializeField] private Image intentRenderer;
    [SerializeField] private TextMeshProUGUI intentPowerText;

    /// <summary>
    /// Updates the sprite of the intent icon to reflect this enemy's intent.
    /// </summary>
    public void DisplayIntent(UnitAction intent)
    {
        intentShaker.isShaking = false;
        if (intent.power <= 1)
            intentPowerText.gameObject.SetActive(false);
        else
        {
            intentPowerText.gameObject.SetActive(true);
            intentPowerText.text = intent.power.ToString();
        }

        intentRenderer.sprite = intent.icon;
    }
}
