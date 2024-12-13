using System.Collections;
using UnityEngine;

public class ASM_State_DashAwayFromPlayer : IState
{
    private float dashSpeed;
    private float dashDistance;
    private float delayForEnemy;
    private AlienSwordmasterReferences references;
    private Transform playerTransform;
    private bool dashDone;
    private ParticleSystem dashParticle;
    private Renderer enemyRenderer;

    public ASM_State_DashAwayFromPlayer(AlienSwordmasterReferences references)
    {
        this.references = references;
        this.dashDistance = references.DashAwayDistance;
        dashParticle = references.DashingParticleSystem;
        enemyRenderer = references.EnemyRenderer;
        dashSpeed = references.DashSpeed;
        delayForEnemy = references.DelayAfterDashParticles;
    }

    public void OnEnter()
    {
        if (references.NumberOfAttacksDone > references.NumberOfAttacksBeforeDashingAway)
            references.NumberOfAttacksDone = 0;

        references.IsAttacing = false;

        dashDone = false;
        playerTransform = references.Character.transform;

        float travelTime = dashDistance / dashSpeed;

        var particleMain = dashParticle.main;
        particleMain.startLifetime = travelTime * delayForEnemy;
        particleMain.startSpeed = dashDistance / (travelTime * delayForEnemy);

        dashParticle.Play();
        enemyRenderer.enabled = false;
        references.EnemySword.SetActive(false);

        references.Mono.StartCoroutine(DashAwayFromPlayer(travelTime));
    }

    private IEnumerator DashAwayFromPlayer(float travelTime)
    {
        Vector3 direction = (references.transform.position - playerTransform.position).normalized;
        Vector3 targetPosition = references.transform.position + direction * dashDistance;

        float elapsedTime = 0f;
        Vector3 startPosition = references.transform.position;

        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            references.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / travelTime);
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
        dashParticle.Stop();
        enemyRenderer.enabled = true;
    }

    public void Tick()
    {
    }

    public bool IsDone() => dashDone;
}