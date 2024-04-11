using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any unit that fights the player.
/// </summary>
[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : Unit
{
    [SerializeField] private List<ActionConstructor> actionConstructors;

    /// <summary>The action this enemy will take at the start of its turn.</summary>
    public UnitAction intent { get; protected set; }

    public List<EnemyPart> possiblePartRewards;

    public override IEnumerator TurnRoutine()
    {
        if (hp > 0)
            yield return intent.OnAct(this, GetTargetsFromActionTargetType(intent.targetType)); 
    }
    /// <param name="type">The targetType of the action</param>
    /// <returns></returns>
    protected List<Unit> GetTargetsFromActionTargetType(UnitAction.TargetType type)
    {
        switch(type)
        {
            case UnitAction.TargetType.randomEnemy: 
            case UnitAction.TargetType.enemy: 
            case UnitAction.TargetType.allEnemies:
                return new List<Unit> { Player.instance };

            case UnitAction.TargetType.self:
                return new List<Unit> { this };
            default:
                Debug.LogWarning("Unknown Target Type: " + type);
                return new List<Unit> { };
        }
    }


    /// <summary>
    /// Changes the enemy's intent. This method is to be used at the start of rounds.
    /// </summary>
    public void UpdateIntent()
    {
        intent = actionConstructors[Random.Range(0, actionConstructors.Count)].CreateAction();
        loader.UpdateIntentIcon(intent);
    }

    protected override void Die()
    {
        if (possiblePartRewards.Count > 0)
            ItemRewardsMenu.pendingRewards.Add(GetPartReward());

        FightManager.enemies.Remove(this);
        if (FightManager.enemies.Count == 0)
            Player.instance.EndTurn();
    }

    private EnemyPart GetPartReward()
    {
        return Instantiate(possiblePartRewards[Random.Range(0, possiblePartRewards.Count)]);
    }
}
