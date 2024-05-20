using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, ITargetable target)
    {
        yield return new WaitForSeconds(0.2f);
    }
}
