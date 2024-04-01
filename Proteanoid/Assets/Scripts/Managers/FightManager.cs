using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    public static List<Enemy> enemies = new();
    [SerializeField] private Transform fightCameraPos;
    [SerializeField] private List<UnitLoader> enemyLoaders;

    [SerializeField] private List<Enemy> testEnemies;

    public static event Action OnRoundStart;
    public static event Action OnFightStart;
    private void Start()
    {
        StartCoroutine(StartFightRoutine(testEnemies));
    }
    public IEnumerator StartFightRoutine(List<Enemy> enemiesInFight) {

        CardManager.Instance.ResetCards();
        CardManager.Instance.DrawInnateCards();

        for(int i=0; i<enemyLoaders.Count; i++)
        {
            if (enemiesInFight.Count > i + 1)
                enemyLoaders[i].LoadNewUnit(enemiesInFight[i]);
            else
                enemyLoaders[i].gameObject.SetActive(false);
        }
        yield return CameraMovement.Instance.MoveToPos(fightCameraPos.position, 1);
        OnFightStart?.Invoke();
        yield return TurnCycleRoutine();

    }

    /// <summary>
    /// Waits for the player's turn, then waits for each enemy's turn. Repeat until the fight ends.
    /// </summary>
    private IEnumerator TurnCycleRoutine()
    {
        while(enemies.Count > 0)
        {
            OnRoundStart?.Invoke();

            foreach (Enemy enemy in enemies)
                enemy.UpdateIntent();

            Player.instance.OnStartTurn();
            yield return Player.instance.TurnRoutine();

            foreach (Enemy enemy in new List<Enemy>(enemies))
            {
                yield return new WaitForSeconds(0.4f);
                enemy.OnStartTurn();
                yield return new WaitForSeconds(0.1f);
                if (enemy != null)
                {
                    yield return enemy.TurnRoutine();
                }
            }

            foreach(Enemy enemy in enemies)
                enemy.OnRoundEnd();
            Player.instance.OnRoundEnd();

            yield return new WaitForSeconds(0.5f);
        }
    }


}
