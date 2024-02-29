using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, List<Unit> targets)
    {
        yield return actor.movement.MoveToAggroPos();
        foreach (Unit target in targets)
            target.TakeDamage(power, true);
    }
}
