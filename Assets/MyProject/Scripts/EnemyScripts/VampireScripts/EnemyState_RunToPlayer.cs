using UnityEngine;
using UnityEngine.AI;

public class EnemyState_RunToPlayer : IState
{
    private EnemyReferences enemyReferences;
    private CharacterController character;
    private float pathUpdateDeadline;

    public EnemyState_RunToPlayer(EnemyReferences enemyReferences, CharacterController character)
    {
        this.enemyReferences = enemyReferences;
        this.character = character;
    }

    public void OnEnter()
    {
        enemyReferences.NavMeshAgent.SetDestination(character.transform.position);
    }

    public void OnExit()
    {
        enemyReferences.Animator.SetFloat("Speed", 0f);
    }

    public void Tick()
    {
        UpdatePath();
        enemyReferences.Animator.SetFloat("Speed", enemyReferences.NavMeshAgent.desiredVelocity.sqrMagnitude);
    }

    private void UpdatePath()
    {
        if (Time.time < pathUpdateDeadline)
            return;

        pathUpdateDeadline = Time.time + enemyReferences.PathUpdateDelay;
        enemyReferences.NavMeshAgent.SetDestination(character.transform.position);
    }

    public bool HasArrivedAtDestination()
    {
        Debug.Log("Distance: " + enemyReferences.NavMeshAgent.remainingDistance);
        return enemyReferences.NavMeshAgent.remainingDistance < 1.6f;
    }
}