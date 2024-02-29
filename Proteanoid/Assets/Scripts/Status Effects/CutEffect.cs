using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the affected unit takes damage, increase that damage by 1 per stack and remove 1 stack of Cut.
/// </summary>
public class CutEffect : StatusEffect {
    protected override void OnTakeDamage()
    {
        Invoke(nameof(TakeExtraDamage), 0.2f);
    }

    private void TakeExtraDamage()
    {
        affectedUnit.TakeDamage(stacks, false);
        RemoveStacks(1);
    }
}
