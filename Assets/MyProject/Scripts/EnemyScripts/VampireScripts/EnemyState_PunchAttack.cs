using UnityEngine;

public class EnemyState_PunchAttack : IState
{
    private bool punchAttackFinised;
    private EnemyReferences enemyReferences;

    public EnemyState_PunchAttack(EnemyReferences enemyReferences)
    {
        this.enemyReferences = enemyReferences;
        enemyReferences.OnPunchAttackFinished += HandlePunchFinished;
    }

    private void HandlePunchFinished()
    {
        punchAttackFinised = true;
    }

    public void OnEnter()
    {
        punchAttackFinised = false;
        enemyReferences.Animator.SetBool("IsPunching", true);
    }

    public void OnExit()
    {
        enemyReferences.Animator.SetBool("IsPunching", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => punchAttackFinised;
}