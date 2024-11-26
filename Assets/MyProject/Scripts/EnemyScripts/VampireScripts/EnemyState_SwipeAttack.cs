using UnityEngine;

public class EnemyState_SwipeAttack : IState
{
    private EnemyReferences enemyReferences;
    private bool swipeAttackFinished;

    public EnemyState_SwipeAttack(EnemyReferences enemyReferences)
    {
        this.enemyReferences = enemyReferences;
        enemyReferences.OnSwipeAttackFinished += HandleSwipeAttackFinished;
    }

    private void HandleSwipeAttackFinished()
    {
        swipeAttackFinished = true;
    }

    public void OnEnter()
    {
        swipeAttackFinished = false;
        enemyReferences.Animator.SetBool("IsSwipeAttacking", true);
    }

    public void OnExit()
    {
        enemyReferences.Animator.SetBool("IsSwipeAttacking", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => swipeAttackFinished;
}