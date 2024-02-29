using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAmogus : Enemy
{
    protected override void UpdateIntent()
    {
        intent = new RedAmogusStab(damage);
        intentIcon.DisplayIntent(intent, GetPredictedDamage());
    }
}
