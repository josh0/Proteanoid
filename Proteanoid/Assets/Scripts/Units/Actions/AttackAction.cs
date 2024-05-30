using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Attack")]
public class AttackAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, ITargetable target)
    {
        target.OnAttack(power + damageModifier);
        ApplyEffectToTarget(target);
        yield return new WaitForSeconds(0.15f);
    }
}
