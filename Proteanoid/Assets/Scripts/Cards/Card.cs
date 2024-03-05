using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public List<UnitAction> actions;

    public string cardName;
    public int manaCost;

    private Unit attackTarget;
    private CardButton cardButton;

    /// <summary>Activates all actions on the card. Should be called AFTER OnSelect().</summary>
    public IEnumerator OnPlay()
    {
        cardButton.SetHeldCard(null);
        if (attackTarget == null)
            Debug.LogError("Tried to play an attack, but no target was declared.");
        foreach(UnitAction action in actions)
        {
            yield return action.OnAct(Player.instance, GetTargetsFromActionTargetType(action.targetType));
        }
        CardManager.Instance.SetCardsInteractable(true);
    }

    public IEnumerator OnSelect()
    {
        CardManager.Instance.SetCardsInteractable(false);
        if (IsCardManuallyTargeted()) {
            yield return TargetSelector.Instance.SelectCardTarget(this);
        }
        else
            yield return CardPlayArea.Instance.WaitForMouseUp(this); 
    }

    public void OnDeselect()
    {
        CardManager.Instance.SetCardsInteractable(true);

    }

    public void SetAttackTarget(Enemy target)
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

    public void SetCardButton(CardButton button)
    {
        cardButton = button;
    }

    private List<Unit> GetTargetsFromActionTargetType(UnitAction.TargetType type)
    {
        switch (type)
        {
            case UnitAction.TargetType.enemy:
                return new List<Unit> { attackTarget };

            case UnitAction.TargetType.randomEnemy:
                return new List<Unit> { FightManager.enemies[Random.Range(0, FightManager.enemies.Count)] };

            case UnitAction.TargetType.allEnemies:
                return FightManager.enemies.OfType<Unit>().ToList<Unit>();

            case UnitAction.TargetType.self:
                return new List<Unit> { Player.instance };

            default:
                Debug.LogWarning("Unknown Target Type: " + type);
                return new List<Unit> { };
        }
    }

    /// <summary>
    /// Instantiate a projectile next to the player with a random offset.
    /// </summary>
    /// <param name="prefab">The prefab that should be instantiated.</param>
    /// <returns>A reference to the projectile that was instantiated.</returns>
    protected T InstantiateProjectileAtPlayer<T>(T prefab) where T : Projectile
    {
        Vector2 targetPos = Player.instance.transform.position + Vector3.right * 1.5f + Random.insideUnitSphere;
        return Instantiate(prefab, targetPos, Quaternion.identity);
    }
}
