using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    public List<StatusEffect> effects = new List<StatusEffect>();

    public void TakeDamage(int amount)
    {
        FightManager.enemy.TakeDamage(amount, true);
    }
}
