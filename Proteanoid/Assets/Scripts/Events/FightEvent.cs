using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Events/Fight")]
public class FightEvent : MapEvent
{
    [SerializeField] private Enemy enemyToFight;
    public override IEnumerator EventRoutine()
    {
        yield return FightManager.Instance.FightRoutine(enemyToFight);
    }
}
