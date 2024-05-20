using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable {
    public void AddBlock(int amount);
    public int TakeDamage(int amount, bool procsOnHitEffects);
    public void AddEffect(StatusEffect effect, int stacks);
    public void RemoveEffect<T>() where T : StatusEffect;

}
