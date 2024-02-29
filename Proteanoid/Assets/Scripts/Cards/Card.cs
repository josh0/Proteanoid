using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public List<UnitAction> actions;

    public string cardName;
    public string cardDescription;
    public Sprite sprite;

    public int manaCost;

    /// <summary></summary>
    /// <param name="targets">The enemy this card should target (Should be null if not targeting an enemy.)</param>
    public IEnumerator OnPlay()
    {
        foreach(UnitAction action in actions)
        {
            action.OnAct(Player.instance, )
        }
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

    /// <summary>Prompts the player to target an enemy.</summary>
    private IEnumerator GetTarget()
    {
        yield return Player.instance.SelectCardTarget(this);
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
