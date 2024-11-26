using UnityEngine;

public class EnemyState_StartCombat : IState
{
    private EnemyReferences enemyReferences;
    private bool roarAnimationFinished;

    public EnemyState_StartCombat(EnemyReferences enemyReferences)
    {
        this.enemyReferences = enemyReferences;
        enemyReferences.OnRoarAnimationFinished += HandleRoarAnimationFinished;
    }

    private void HandleRoarAnimationFinished()
    {
        roarAnimationFinished = true;
    }

    public void OnEnter()
    {
        roarAnimationFinished = false;
        enemyReferences.Animator.SetBool("Combat", true);
    }

    public void OnExit()
    {
        enemyReferences.Animator.SetBool("Combat", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => roarAnimationFinished;
}