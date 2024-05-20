using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchlingShell : EnemyPart
{
    public override void BreakPart()
    {
        base.BreakPart();
        parentEnemy.block = 0;
        parentEnemy.StunEnemy();
    }
}
