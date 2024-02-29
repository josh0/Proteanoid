using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Any unit that fights the player.
/// </summary>
public abstract class Enemy : Unit
{
    ///<summary>The script that updates this enemy's intent SpriteRenderer and text.</summary>
    [SerializeField] protected EnemyIntentIcon intentIcon;

    /// <summary>The action this enemy will take at the start of its turn.</summary>
    public UnitAction intent { get; protected set; }

    /// <summary>The base amount of damage the enemy deals (before strength, block, and other effects)</summary>.
    [SerializeField] protected int damage;

    [SerializeField] private int maxStunPoints;
    private int stunPoints;

    [SerializeField] private Slider stunPointSlider;

    protected virtual void OnEnable()
    {
        FightManager.enemies.Add(this);
        FightManager.OnRoundStart += UpdateIntent;
    }

    protected virtual void OnDisable()
    {
        FightManager.enemies.Remove(this);
        FightManager.OnRoundStart -= UpdateIntent;
    }

    public override IEnumerator TurnRoutine()
    {
        yield return intent.OnAct(this, intent.); 
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
    protected abstract void UpdateIntent();

    /// <summary>
    /// Tries to deal a given amount of damage to the player.
    /// </summary>
    /// <param name="damage">The amount of damage the enemy should try to deal.</param>
    /// <returns>Whether or not the attack dealt unblocked damage.</returns>
    public bool AttackPlayer(int damage)
    {
        int damageDealt = Player.instance.TakeDamage(damage, true);
        if (damageDealt > 0) return true;
        else return false;
    }

    /// <summary>
    /// Calculates the amount of damage an attack would deal against the player.
    /// </summary>
    /// <returns>The final amount of damage the enemy will deal to the player.</returns>
    public int GetPredictedDamage()
    {
        return damage + strength;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected void AddStunPoints(int amount)
    {
        stunPoints += amount;
        stunPointSlider.value = stunPoints;

        if (stunPoints > maxStunPoints)
            stunPoints = maxStunPoints;
        else if (stunPoints <= 0)
            StunEnemy();
    }

    public void ClearStun()
    {
        stunPoints = maxStunPoints;
        AddStunPoints(0);
    }

    protected void StunEnemy()
    {
        intent = new StunAction();
    }
}
