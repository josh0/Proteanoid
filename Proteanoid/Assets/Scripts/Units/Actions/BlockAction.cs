using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, List<Unit> targets)
    {
        actor.GainBlock(power);
        yield return null;
    }
}
