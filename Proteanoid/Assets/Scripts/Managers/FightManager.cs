using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static List<Enemy> enemies = new();

    public static event Action OnRoundStart;
    public static event Action OnFightStart;

    private void Start()
    {
        StartCoroutine(TurnCycleRoutine());
    }

    /// <summary>
    /// Waits for the player's turn, then waits for each enemy's turn. Repeat until the fight ends.
    /// </summary>
    private IEnumerator TurnCycleRoutine()
    {
        OnFightStart?.Invoke();
        while(enemies.Count > 0)
        {
            OnRoundStart();
            Player.instance.OnStartTurn();
            yield return Player.instance.TurnRoutine();
            StartCoroutine(Player.instance.movement.MoveToOriginalPos());

            foreach (Enemy enemy in new List<Enemy>(enemies))
            {
                yield return new WaitForSeconds(0.4f);
                enemy.OnStartTurn();
                yield return new WaitForSeconds(0.1f);
                if (enemy != null)
                {
                    yield return enemy.TurnRoutine();
                    StartCoroutine(enemy.movement.MoveToOriginalPos());
                }
            }

            foreach(Enemy enemy in enemies)
                enemy.OnRoundEnd();
            Player.instance.OnRoundEnd();

            yield return new WaitForSeconds(0.5f);
        }
    }


}
