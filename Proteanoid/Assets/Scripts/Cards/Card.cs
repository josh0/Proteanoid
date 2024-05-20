using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    [Tooltip("The actions in this list will be added to the card when it is created. (Intended for inspector only)")]
    [SerializeField] private List<ActionConstructor> actionConstructors = new();
    public List<UnitAction> actions { get; private set; } = new();
    public int manaCost;

    private EnemyPart attackTarget;
    private HandCardButton cardButton;

    public enum Keywords
    {
        retain,
        innate,
        exhaust,
        fleeting
    }

    public List<Keywords> keywords = new();

    /// <summary>
    /// Adds the list of constructors that was assigned in the inspector to the list of this card's actions. <br />
    /// Should be used when the card is first offered as a reward.
    /// </summary>
    public void OnCreate()
    {
        foreach (ActionConstructor con in actionConstructors)
        {
            actions.Add(con.CreateAction());
        }
        actionConstructors.Clear();
    }

    /// <summary>Activates all actions on the card. Should be called AFTER OnSelect(), and should only be called by the Player script.</summary>
    public IEnumerator OnPlay()
    {
        if (keywords.Contains(Keywords.exhaust))
            CardManager.Instance.ExhaustCard(this);
        else
            CardManager.Instance.DiscardCard(this);
        foreach (UnitAction action in actions)
        {
            yield return action.OnAct(Player.instance, GetTargetsFromActionTargetType(action.targetType));
            yield return new WaitForSeconds(0.1f);
        }
        CardManager.Instance.SetCardsInteractable(true);
    }

    public IEnumerator OnSelect(CardButtonBehaviour button)
    {
        CardManager.Instance.SetCardsInteractable(false);
        button.SetTargetTransformActive(false);
        if (IsCardManuallyTargeted()) {
            yield return TargetSelector.Instance.SelectCardTarget(this, button);
        }
        else
            yield return CardPlayArea.Instance.WaitForMouseUp(this, button); 
    }

    public void OnDeselect()
    {
        CardManager.Instance.SetCardsInteractable(true);
        CardManager.Instance.SetHeldCardButton(null);
        cardButton.behaviour.SetTargetTransformActive(true);
    }

    public void SetAttackTarget(EnemyPart target)
    {
        attackTarget = target;
    }

    /// <returns>Whether or not this card should prompt the player to target an enemy.</returns>
    private bool IsCardManuallyTargeted()
    {
        foreach (UnitAction action in actions)
        {
            if (action.targetType == UnitAction.TargetType.enemy)
                return true;
        }
        return false;
    }

    public void SetCardButton(HandCardButton button)
    {
        cardButton = button;
    }

    private ITargetable GetTargetsFromActionTargetType(UnitAction.TargetType type)
    {
        switch (type)
        {
            case UnitAction.TargetType.enemy:
                return attackTarget;

            case UnitAction.TargetType.self:
                return Player.instance;

            default:
                Debug.LogWarning("Unknown Target Type: " + type);
                return null;
        }
    }
}
