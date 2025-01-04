using System.Collections;
using UnityEngine;

public class AlienGSDashingController : MonoBehaviour
{
    [SerializeField] private float timeForPlayerAttacking = 2f;
    [SerializeField] private float dashDistance = 7f;
    private AlienGunslingerReferences references;
    private bool dashDone;
    private Transform playerTransform;

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

        StartCoroutine(DashCoroutine(dashDirection));
    }

    private IEnumerator DashCoroutine(Vector3 dashDirection)
    {
        yield return new WaitForSeconds(timeForPlayerAttacking);
        transform.position += dashDirection * dashDistance;
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
}