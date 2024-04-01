using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetSelector : Singleton<TargetSelector>
{
    [SerializeField] private Animator selectPromptAnimator;
    [SerializeField] private Transform heldCardPos;
    [SerializeField] private LineRenderer targetLineRenderer;
    public Enemy selectedTarget { get; private set; }

    public void SelectTarget(Enemy target)
    {
        selectedTarget = target;
    }

    /// <summary>
    /// Waits for the player to set selectedTarget to any enemy, then sets the given card's attackTarget to selectedTarget.
    /// </summary>
    public IEnumerator SelectCardTarget(Card card, CardButtonBehaviour button)
    {
        targetLineRenderer.enabled = true;
        selectedTarget = null;
        selectPromptAnimator.SetBool("isDisplayed", true);
        while (selectedTarget == null)
        {
            yield return null;

            button.targetPos = heldCardPos.position;

            targetLineRenderer.SetPosition(0, button.transform.position + Vector3.up);
            targetLineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y, 20)));

            if (Input.GetMouseButtonUp(1))
            {
                card.OnDeselect();
                selectPromptAnimator.SetBool("isDisplayed", false);
                break;
            }
        }

        targetLineRenderer.enabled = false;

        //This null check is here in case the while loop is cancelled with right click.
        if (selectedTarget != null)
        {
            card.SetAttackTarget(selectedTarget);
            CardManager.Instance.PlayCard(card);
            selectPromptAnimator.SetBool("isDisplayed", false);
        }
    }
}
