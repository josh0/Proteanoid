using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the affected unit takes damage, increase that damage by 1 per stack and remove 1 stack of Cut.
/// </summary>
public class CutEffect : StatusEffect {
    public override void OnTakeDamage(Unit affectedUnit)
    {
        affectedUnit.TakeDamage(stacks, false);
        RemoveStacks(1);
    }
}
