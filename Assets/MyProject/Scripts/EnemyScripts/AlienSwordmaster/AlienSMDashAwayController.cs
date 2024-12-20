using System.Collections;
using UnityEngine;

public class AlienSMDashAwayController : MonoBehaviour
{
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private float dashChanceInSecondPhase = 0.7f;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashDistance = 9;
    [SerializeField] private float delayForEnemy = 1f;
    [SerializeField] private GameObject enemySword;
    [SerializeField] private ParticleSystem dashParticle;
    private AlienSwordmasterReferences references;
    private Transform playerTransform;
    private bool dashDone;


    private void Awake()
    {
        references = GetComponentInParent<AlienSwordmasterReferences>();
    }

    public void Dash()
    {
        dashDone = false;
        playerTransform = references.Character.transform;

        float travelTime = dashDistance / dashSpeed;

        var particleMain = dashParticle.main;
        particleMain.startLifetime = travelTime * delayForEnemy;
        particleMain.startSpeed = dashDistance / (travelTime * delayForEnemy);

        dashParticle.Play();
        enemyRenderer.enabled = false;
        enemySword.SetActive(false);

        StartCoroutine(DashAwayFromPlayer(travelTime));
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
        enemySword.SetActive(true);
        enemyRenderer.enabled = true;
    }

    public bool IsDone() => dashDone;
    public float GetDashChance() => dashChanceInSecondPhase;
}