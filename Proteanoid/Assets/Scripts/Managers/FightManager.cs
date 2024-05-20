using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    public static Enemy enemy;
    [SerializeField] private Transform fightCameraPos;

    [SerializeField] private Animator fightUIAnimator;
    [SerializeField] private Vector3 enemyPos;

    public static event Action OnRoundStart;
    public static event Action OnFightStart;
    public IEnumerator FightRoutine(Enemy newEnemy) {
        fightUIAnimator.SetBool("isMenuOpen", true);

        CardManager.Instance.ResetCards();
        CardManager.Instance.DrawInnateCards();

        enemy = Instantiate(newEnemy, enemyPos, Quaternion.identity);

        yield return CameraMovement.Instance.MoveToPos(fightCameraPos.position, 1);
        OnFightStart?.Invoke();
        yield return TurnCycleRoutine();

        fightUIAnimator.SetBool("isMenuOpen", false);

        yield return ItemRewardsMenu.Instance.GetRewardsRoutine();
        StartCoroutine(CameraMovement.Instance.MoveToPos(CameraMovement.Instance.mapPos.position, 1));
        Destroy(enemy.gameObject);
    }

    /// <summary>
    /// Waits for the player's turn, then waits for each enemy's turn. Repeat until the fight ends.
    /// </summary>
    private IEnumerator TurnCycleRoutine()
    {
        while (enemy.hp > 0)
        {
            OnRoundStart?.Invoke();

            enemy.UpdateIntent();

            //Player turn
            Player.instance.OnStartTurn();
            yield return Player.instance.TurnRoutine();

            //Enemy turn
            enemy.OnStartTurn();
            yield return new WaitForSeconds(0.1f);
            yield return enemy.TurnRoutine();
            yield return new WaitForSeconds(0.3f);

            //End round
            enemy.OnRoundEnd();
            Player.instance.OnRoundEnd();

            yield return new WaitForSeconds(0.5f);
        }
    }


}
