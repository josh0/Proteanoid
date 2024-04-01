using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Attack")]
public class AttackAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, List<Unit> targets)
    {
        foreach (Unit target in targets)
            if (target != null)
            {
                target.TakeDamage(power + damageModifier, true);
                ApplyEffectToTarget(target);
            }
        yield return null;
    }
}
