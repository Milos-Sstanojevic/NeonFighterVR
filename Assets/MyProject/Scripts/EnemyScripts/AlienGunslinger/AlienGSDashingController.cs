using System.Collections;
using Mono.Cecil;
using UnityEngine;

public class AlienGSDashingController : MonoBehaviour
{
    [SerializeField] private float dashDistance = 7f;
    [SerializeField] private GameObject enemyMesh;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject guns;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private ParticleSystem dashParticle;
    private AlienGunslingerReferences references;
    private Transform playerTransform;
    private bool isDashing;
    private Coroutine dashingCoroutine;
    private bool dashDone;

    private void Awake()
    {
        references = GetComponent<AlienGunslingerReferences>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnShieldBrokenAction(ShieldBroken);
    }

    private void ShieldBroken()
    {
        StopAllCoroutines();
    }

    public void DashAwayFromPlayer()
    {
        transform.position += -references.Character.transform.forward * dashDistance;
    }

    public void Dash()
    {
        playerTransform = references.Character.transform;

        Vector3 dashDirection = Vector3.zero;

        dashDirection = CalculateDashDirection();

        if (dashDirection == Vector3.zero)
        {
            Debug.LogError("Dash direction is zero");
            return;
        }

        float travelTime = dashDistance / dashSpeed;
        var particleMain = dashParticle.main;
        particleMain.startLifetime = travelTime;
        particleMain.startSpeed = dashDistance / travelTime;

        dashParticle.Play();
        enemyMesh.SetActive(false);
        shield.SetActive(false);
        guns.SetActive(false);

        StartCoroutine(DashCoroutine(dashDirection, travelTime));

        // transform.position += dashDirection * dashDistance;

        // dashDone = true;
    }

    private IEnumerator DashCoroutine(Vector3 direction, float travelTime)
    {
        Vector3 targetPosition = transform.position + direction * dashDistance;

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / travelTime);
            yield return null;
        }

        transform.position = targetPosition;
        dashDone = true;

        dashParticle.Stop();
        enemyMesh.SetActive(true);
        shield.SetActive(true);
        guns.SetActive(true);
    }

    private Vector3 CalculateDashDirection()
    {
        int randomDirection = Random.Range(0, 3);
        switch (randomDirection)
        {
            case 0:
                return -playerTransform.right;
            case 1:
                return playerTransform.right;
            case 2:
                return -playerTransform.forward;
            default:
                return Vector3.zero;
        }
    }

    public void StartDashingCoroutine()
    {
        dashingCoroutine = StartCoroutine(DashingCoroutine());
    }

    private IEnumerator DashingCoroutine()
    {
        yield return new WaitForSeconds(references.TimeForPlayerAttacking);
        isDashing = true;
    }

    public void StopDashingCoroutine()
    {
        isDashing = false;
        StopCoroutine(dashingCoroutine);
    }

    public bool IsDashing() => isDashing;
    public bool GetDashDone() => dashDone;
    public void ResetDashDone() => dashDone = false;
}