using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Block")]
public class BlockAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, ITargetable target)
    {
        target.AddBlock(power);
        yield return new WaitForSeconds(0.1f);
    }
}
