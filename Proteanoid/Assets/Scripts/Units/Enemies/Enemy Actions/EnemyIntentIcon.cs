using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

/// <summary>
/// Controls the SpriteRenderer, TextMeshPro, and ShakeMovement of an enemy's intent icon.
/// </summary>

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyIntentIcon : MonoBehaviour
{
    [SerializeField] private ShakeMovement intentShaker;

    [SerializeField] private SpriteRenderer intentRenderer;
    [SerializeField] private TextMeshPro intentDamageText;

    [SerializeField] private Sprite attackIntentSprite;
    [SerializeField] private Sprite buffIntentSprite;
    [SerializeField] private Sprite debuffIntentSprite;
    [SerializeField] private Sprite specialIntentSprite;

    /// <summary>
    /// Updates the sprite of the intent icon to reflect this enemy's intent.
    /// </summary>
    public void DisplayIntent(UnitAction intent, int predictedDamage)
    {
        intentShaker.isShaking = false;
        if (predictedDamage == 0)
            intentDamageText.gameObject.SetActive(false);
        else
        {
            intentDamageText.gameObject.SetActive(true);
            intentDamageText.text = predictedDamage.ToString();
        }

        switch (intent.type)
        {
            case UnitAction.Type.attack:
                intentRenderer.sprite = attackIntentSprite;
                if (predictedDamage >= 30)
                    intentShaker.isShaking = true;
                break;
            case UnitAction.Type.buff:
                intentRenderer.sprite = buffIntentSprite;
                break;
        }
    }
}
