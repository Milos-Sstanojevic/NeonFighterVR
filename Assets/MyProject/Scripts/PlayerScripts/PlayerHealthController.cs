using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

//needs refactoring
public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private float minimumBigDamage = 30;
    [SerializeField] private DynamicMoveProvider dynamicMoveProvider;
    [SerializeField] private ContinuousTurnProvider continuousTurnProvider;
    [SerializeField] private float debuffedMovementSpeed;
    [SerializeField] private float debuffedRotationSpeed;
    [SerializeField] private float debuffTimeLast = 10f;
    [SerializeField] private float knockbackDistance = 2f; // Distance to move player backward
    [SerializeField] private float knockbackSpeed = 5f; // Speed of knockback
    // [SerializeField] private Transform cameraTransform;  // Reference to the camera
    [SerializeField] private float shakeDuration = 0.2f; // Duration of camera shake
    [SerializeField] private float shakeMagnitude = 0.05f; // Intensity of camera shake

    private float originalSpeed;
    private float originalRotationSpeed;

    private int maxHealth = 100;
    private int currentHealth = 100;

    private void Awake()
    {
        originalSpeed = dynamicMoveProvider.moveSpeed;
        originalRotationSpeed = continuousTurnProvider.turnSpeed;
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPlayerHitAction(TakeDamage);
    }

    private void TakeDamage(int damage)
    {
        if (damage >= minimumBigDamage)
        {
            StartCoroutine(DebuffPlayer());
            StartCoroutine(KnockbackPlayer());
            StartCoroutine(ShakeCamera());
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
            Debug.Log("Died");
    }

    private IEnumerator DebuffPlayer()
    {
        dynamicMoveProvider.moveSpeed = debuffedMovementSpeed;
        continuousTurnProvider.turnSpeed = debuffedRotationSpeed;

        yield return new WaitForSeconds(debuffTimeLast);

        dynamicMoveProvider.moveSpeed = originalSpeed;
        continuousTurnProvider.turnSpeed = originalRotationSpeed;
    }

    private IEnumerator KnockbackPlayer()
    {
        Vector3 knockbackDirection = -transform.forward;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + knockbackDirection * knockbackDistance;

        float elapsedTime = 0f;

        while (elapsedTime < knockbackDistance / knockbackSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * knockbackSpeed) / knockbackDistance);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator ShakeCamera()
    {
        Vector3 originalRotation = transform.localEulerAngles;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            float z = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.localEulerAngles = new Vector3(originalRotation.x + x, originalRotation.y + y, originalRotation.z + z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localEulerAngles = originalRotation;

    }
}