using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When the affected unit takes damage, increase that damage by 1 per stack. Lasts 1 round.
/// </summary>
public class CutEffect : StatusEffect {
    public override void OnTakeDamage(Unit affectedUnit)
    {
        affectedUnit.TakeDamage(stacks, false);
    }

    public override void OnRoundEnd(Unit affectedUnit)
    {
        affectedUnit.RemoveEffect(this);
    }
}
