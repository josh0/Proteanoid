using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Block")]
public class BlockAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, List<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            target.GainBlock(power);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
