using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAmogus : Enemy
{
    [SerializeField] private List<UnitAction> unitActions;
    protected override void UpdateIntent()
    {
        intent = unitActions[Random.Range(0, unitActions.Count)];
        intentIcon.DisplayIntent(intent);
    }
}
