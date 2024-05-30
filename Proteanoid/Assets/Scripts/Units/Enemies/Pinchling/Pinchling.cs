using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pinchling : Enemy
{
    //If the Pinchling has less than 10 block and has a shell, it gains block. Otherwise, it attacks.
    public override void UpdateIntent()
    {
        BlockAction blockAction = GetActionFromParts<BlockAction>();
        if (block < 10 && blockAction != null)
            intent = blockAction;
        else
            intent = GetActionFromParts<AttackAction>();

        base.UpdateIntent();
    }
}