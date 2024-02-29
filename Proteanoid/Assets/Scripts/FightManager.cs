using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static List<Enemy> enemies = new();

    public static event Action OnRoundStart;
    public static event Action OnRoundEnd;

    private void Start()
    {
        StartCoroutine(TurnCycleRoutine());
    }

    /// <summary>
    /// Waits for the player's turn, then waits for each enemy's turn. Repeat until the fight ends.
    /// </summary>
    private IEnumerator TurnCycleRoutine()
    {
        while(enemies.Count > 0)
        {
            OnRoundStart();
            Player.instance.CallOnStartTurn();
            yield return Player.instance.TurnRoutine();

            foreach (Enemy enemy in enemies)
            {
                yield return new WaitForSeconds(0.4f);
                enemy.CallOnStartTurn();
                yield return enemy.TurnRoutine();
            }


            OnRoundEnd();
            yield return new WaitForSeconds(0.5f);
        }
    }


}
