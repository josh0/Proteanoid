using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Assimilate")]
public class AssimilateAction : UnitAction
{
    public override IEnumerator OnAct(Unit actor, List<Unit> targets)
    {
        foreach(Unit target in targets)
            if (target is Enemy enemy)
            {
                yield return AssimilateMenu.Instance.SelectRewards(enemy.rewards);
            }
    }
}
