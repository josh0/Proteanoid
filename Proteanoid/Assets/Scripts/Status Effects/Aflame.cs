using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aflame : StatusEffect
{
    protected override void OnStartTurn()
    {
        affectedUnit.TakeDamage(1, true);
    }
}
