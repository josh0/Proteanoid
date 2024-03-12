using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Poisoned")]
public class Poisoned : StatusEffect
{
    public override void OnStartTurn(Unit affectedUnit)
    {
        affectedUnit.TakeDamage(1, true);
        RemoveStacks(1);
    }
}
