using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    public static List<Enemy> enemies = new();
    [SerializeField] private Transform fightCameraPos;
    [SerializeField] private List<UnitLoader> enemyLoaders;

    [SerializeField] private Animator fightUIAnimator;

    [SerializeField] private List<Enemy> testEnemies;

    public static event Action OnRoundStart;
    public static event Action OnFightStart;
    public IEnumerator FightRoutine(List<Enemy> enemiesInFight) {
        fightUIAnimator.SetBool("isMenuOpen", true);

        CardManager.Instance.ResetCards();
        CardManager.Instance.DrawInnateCards();

        for(int i=0; i<enemyLoaders.Count; i++)
        {
            if (enemiesInFight.Count >= i + 1)
                enemyLoaders[i].LoadNewUnit(enemiesInFight[i]);
            else
                enemyLoaders[i].gameObject.SetActive(false);
        }
        yield return CameraMovement.Instance.MoveToPos(fightCameraPos.position, 1);
        OnFightStart?.Invoke();
        yield return TurnCycleRoutine();

        fightUIAnimator.SetBool("isMenuOpen", false);

        yield return ItemRewardsMenu.Instance.GetRewardsRoutine();
        StartCoroutine(CameraMovement.Instance.MoveToPos(CameraMovement.Instance.mapPos.position, 1));
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

            //Player turn
            Player.instance.OnStartTurn();
            yield return Player.instance.TurnRoutine();

            //Enemy turns
            foreach (Enemy enemy in new List<Enemy>(enemies))
            {
                yield return new WaitForSeconds(0.4f);
                enemy.OnStartTurn();
                yield return new WaitForSeconds(0.1f);

                if (enemies.Contains(enemy)) //Check if the enemy died in case status effects killed them
                {
                    yield return enemy.TurnRoutine();
                    Debug.Log("Waiting for " + enemy.name + "'s turn.");
                }
            }

            //End round
            foreach(Enemy enemy in enemies) 
                enemy.OnRoundEnd();
            Player.instance.OnRoundEnd();

            yield return new WaitForSeconds(0.5f);
        }
    }


}
