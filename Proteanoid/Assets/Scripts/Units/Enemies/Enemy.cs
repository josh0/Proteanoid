using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any unit that fights the player.
/// </summary>
public class Enemy : Unit
{
    [SerializeField] private List<ActionConstructor> actionConstructors;

    /// <summary>The action this enemy will take at the start of its turn.</summary>
    public UnitAction intent { get; protected set; }

    public List<Item> possiblePartRewards;
    private UnitVfx vfx;

    public override IEnumerator TurnRoutine()
    {
        if (hp > 0)
            yield return intent.OnAct(this, GetTargetsFromActionTargetType(intent.targetType)); 
    }

    /// <param name="type">The targetType of the action</param>
    /// <returns>The unit the given action should target</returns>
    protected Unit GetTargetsFromActionTargetType(UnitAction.TargetType type)
    {
        switch(type)
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
    /// Changes the enemy's intent. This method is to be used at the start of rounds.
    /// </summary>
    public void UpdateIntent()
    {
        intent = actionConstructors[Random.Range(0, actionConstructors.Count)].CreateAction();
        vfx.UpdateIntentIcon(intent);
    }

    /// <summary>Sets this unit's intent to the stun action</summary>
    public void StunEnemy()
    {
        intent = ScriptableObject.CreateInstance<StunAction>();
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
}
