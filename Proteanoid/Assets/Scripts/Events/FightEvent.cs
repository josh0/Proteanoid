using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEvent : MapEvent
{
    [SerializeField] private List<Enemy> enemiesInFight;
    public override IEnumerator EventRoutine()
    {
        yield return FightManager.Instance.StartFightRoutine(enemiesInFight);
    }
}
