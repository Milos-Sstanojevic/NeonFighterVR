using System.Collections;
using UnityEngine;

public class AlienGSDashingController : MonoBehaviour
{
    [SerializeField] private float dashDistance = 7f;
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
        transform.position += -playerTransform.forward * dashDistance;
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
        transform.position += dashDirection * dashDistance;

        dashDone = true;
    }

    private Vector3 CalculateDashDirection()
    {
        int randomDirection = Random.Range(0, 3);
        switch (randomDirection)
        {
            case 0:
                return -playerTransform.right; // Dash to the left
            case 1:
                return playerTransform.right; // Dash to the right
            case 2:
                return -playerTransform.forward; // Dash to the back
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