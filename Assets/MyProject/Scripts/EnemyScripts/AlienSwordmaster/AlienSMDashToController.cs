using System.Collections;
using UnityEngine;

public class AlienSMDashToController : MonoBehaviour
{
    [SerializeField] private float delayForEnemy = 1f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float stopDistance = 8f;
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private GameObject enemySword;
    private bool dashDone;
    private ParticleSystem dashParticle;
    private AlienSwordmasterReferences references;
    private Transform playerTransform;

    private void Awake()
    {
        references = GetComponentInParent<AlienSwordmasterReferences>();
        dashParticle = GetComponent<ParticleSystem>();
        stopDistance = references.AttackRange - 0.3f;
    }

    public void Dash()
    {
        dashDone = false;
        playerTransform = references.Character.transform;

        float distanceToPlayer = Vector3.Distance(references.transform.position, playerTransform.position) - stopDistance;
        float travelTime = distanceToPlayer / dashSpeed;

        var particleMain = dashParticle.main;
        particleMain.startLifetime = travelTime * delayForEnemy;
        particleMain.startSpeed = distanceToPlayer / (travelTime * delayForEnemy);

        dashParticle.Play();
        enemyRenderer.enabled = false;
        enemySword.SetActive(false);

        StartCoroutine(DashToPlayer(travelTime));
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
        enemySword.SetActive(true);
        enemyRenderer.enabled = true;
    }

    public bool IsDone() => dashDone;
    public bool ShouldDashToPlayer() => Vector3.Distance(references.transform.position, references.Character.transform.position) > stopDistance;

}