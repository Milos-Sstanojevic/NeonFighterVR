using System.Collections;
using UnityEngine;

public class ASM_State_DashToPlayer : IState
{
    private float delayForEnemy;
    private AlienSwordmasterReferences references;
    private Transform playerTransform;
    private float dashSpeed;
    private bool dashDone;
    private float stopDistance;
    private ParticleSystem dashParticle;
    private Renderer enemyRenderer;


    public ASM_State_DashToPlayer(AlienSwordmasterReferences references)
    {
        this.references = references;
        dashParticle = references.DashingParticleSystem;
        enemyRenderer = references.EnemyRenderer;
        stopDistance = references.AttackRange;
        dashSpeed = references.DashSpeed;
        delayForEnemy = references.DelayAfterDashParticles;
    }

    public void OnEnter()
    {
        dashDone = false;
        playerTransform = references.Character.transform;
        references.NavMeshAgent.isStopped = true;

        float distanceToPlayer = Vector3.Distance(references.transform.position, playerTransform.position) - stopDistance;
        float travelTime = distanceToPlayer / dashSpeed;

        var particleMain = dashParticle.main;
        particleMain.startLifetime = travelTime * delayForEnemy;
        particleMain.startSpeed = distanceToPlayer / (travelTime * delayForEnemy);

        dashParticle.Play();
        enemyRenderer.enabled = false;
        references.EnemySword.SetActive(false);

        references.Mono.StartCoroutine(DashToPlayer(travelTime));
    }

    private IEnumerator DashToPlayer(float travelTime)
    {
        Vector3 direction = (playerTransform.position - references.transform.position).normalized;
        Vector3 targetPosition = playerTransform.position - direction * stopDistance;

        float elapesedTime = 0f;
        Vector3 startPosition = references.transform.position;

        while (elapesedTime < travelTime)
        {
            elapesedTime += Time.deltaTime;
            references.transform.position = Vector3.Lerp(startPosition, targetPosition, elapesedTime / travelTime);
            yield return null;
        }

        references.transform.position = targetPosition;
        dashDone = true;

        dashParticle.Stop();
        references.EnemySword.SetActive(true);
        enemyRenderer.enabled = true;
    }

    public void OnExit()
    {
        references.NavMeshAgent.isStopped = false;

        dashParticle.Stop();
        enemyRenderer.enabled = true;
    }

    public void Tick()
    {
    }

    public bool IsDone() => dashDone;
}