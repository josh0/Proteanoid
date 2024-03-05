using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetSelector : Singleton<TargetSelector>
{
    [SerializeField] private Animator selectPromptAnimator;
    public Enemy selectedTarget { get; private set; }

    public void SelectTarget(Enemy target)
    {
        selectedTarget = target;
    }

    /// <summary>
    /// Waits for the player to set selectedTarget to any enemy, then sets the given card's attackTarget to selectedTarget.
    /// </summary>
    public IEnumerator SelectCardTarget(Card card)
    {
        selectedTarget = null;
        selectPromptAnimator.SetBool("isDisplayed", true);
        while (selectedTarget == null)
        {
            yield return null;
        }
        card.SetAttackTarget(selectedTarget);
        Player.instance.PlayCard(card);
        selectPromptAnimator.SetBool("isDisplayed", false);
    }
}
