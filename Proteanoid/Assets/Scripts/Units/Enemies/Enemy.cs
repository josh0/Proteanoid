using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Any unit that fights the player.
/// </summary>
public abstract class Enemy : Unit
{
    /// <summary>All this enemy's unbroken parts.</summary>
    [SerializeField] protected List<EnemyPart> parts = new();

    /// <summary>The action this enemy will take at the start of its turn.</summary>
    public UnitAction intent { get; protected set; }

    public List<Item> possiblePartRewards;

    public override IEnumerator TurnRoutine()
    {
        if (hp > 0)
            yield return intent.OnAct(this, GetTargetsFromActionTargetType(intent.targetType));
    }

    /// <param name="type">The targetType of the action</param>
    /// <returns>The unit the given action should target</returns>
    protected Unit GetTargetsFromActionTargetType(UnitAction.TargetType type)
    {
        switch (type)
        {
            case UnitAction.TargetType.enemy:
                return Player.instance;

            case UnitAction.TargetType.self:
                return this;
            default:
                Debug.LogWarning("Unknown Target Type: " + type);
                return null;
        }
    }


    /// <summary>
    /// Changes the enemy's intent. This method is to be used at the start of rounds. <br />
    /// Remember to include base.UpdateIntent() at the END of overrides.
    /// </summary>
    public virtual void UpdateIntent()
    {
        if (intent == null)
            StunEnemy();

        vfx.UpdateIntentIcon(intent);
    }

    /// <summary>Sets this unit's intent to the stun action</summary>
    public void StunEnemy()
    {
        intent = ScriptableObject.CreateInstance<StunAction>();
    }

    /// <summary>
    /// Checks for blocking parts and other things that might alter the outcome of an attack, then damages the given part.
    /// </summary>
    /// <param name="part">The part that was attacked.</param>
    /// <param name="amount">The amount of pre-mititgation damage to deal to that part.</param>
    /// <param name="procsOnHitEffects">Whether or not this damage should proc on-hit effects.</param>
    /// <returns></returns>
    public virtual int OnAttackPart(EnemyPart part, int amount, bool procsOnHitEffects)
    {
        //Block attack if this enemy has an available blocking part.
        EnemyPart blockingPart = GetPartWithAttribute(EnemyPart.EnemyPartAttributes.blocking);
        if (blockingPart != null)
            return blockingPart.TakeDamage(amount, true);

        //Damage this enemy directly if the part was vital.
        if (part.attributes.Contains(EnemyPart.EnemyPartAttributes.vital))
            return TakeDamage(amount, true);

        //Damage the part if neither of the above is true.
        return part.TakeDamage(amount, procsOnHitEffects);
    }

    /// <summary>
    /// Adds rewards to the Item Rewards pool and ends combat.
    /// </summary>
    protected override void Die()
    {
        if (possiblePartRewards.Count > 0)
            ItemRewardsMenu.pendingRewards.Add(GetPartReward());

        Player.instance.EndTurn();
    }

    private Item GetPartReward()
    {
        return Instantiate(possiblePartRewards[Random.Range(0, possiblePartRewards.Count)]);
    }

    /// <summary>
    /// Iterates through each of this enemy's parts to find an action of type T, then returns the action of that type with the highest power. <br />
    /// If no actions of type T are found, instead return null.
    /// </summary>
    /// <typeparam name="T">The type of action to be returned.</typeparam>
    protected T GetActionFromParts<T>() where T : UnitAction
    {
        List<UnitAction> foundActions = parts.SelectMany(part => part.addedActions)
                            .Where(constructor => constructor.action is T)
                            .Select(constructor => constructor.CreateAction())
                            .ToList();

        if (foundActions.Count == 0)
        {
            Debug.LogWarning("No actions of the given type were found.");
            return null;
        }

        else if (foundActions.Count == 1)
            return foundActions[0] as T;

        //Checking for which action among the found actions has the highest power
        UnitAction highestPowerAction = foundActions[0];

        foreach(UnitAction action in foundActions)
            if (action.power > highestPowerAction.power)
                highestPowerAction = action;

        return highestPowerAction as T;
    }

    /// <summary>Return an unbroken part with the given attribute.</summary>
    protected EnemyPart GetPartWithAttribute(EnemyPart.EnemyPartAttributes attribute)
    {
        foreach (EnemyPart part in parts)
            if (!part.isPartBroken && part.attributes.Contains(attribute))
                return part;
        return null;
    }
}
