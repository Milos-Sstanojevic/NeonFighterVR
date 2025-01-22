using System.Collections;
using UnityEngine;

public class AlienGSSideToSideShootController : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private int shootingAngle = 160;
    [SerializeField] private float rotationSpeed = 2;
    private AlienGunslingerReferences references;
    private CharacterController characterController;
    private float currentAngle;
    private bool isShooting;
    private int rotationDirection = 0;
    private bool doneShooting;
    private bool shotPlayer;

    private void Start()
    {
        references = GetComponent<AlienGunslingerReferences>();
        characterController = references.Character;
    }


    public void StartShooting()
    {
        shotPlayer = false;
        currentAngle = 0f;

        Vector3 directionToPlayer = (characterController.transform.position - transform.position).normalized;

        float midpointRotationY = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

        float startAngleOffset = shootingAngle / 2;
        if (directionToPlayer.x > 0)
        {
            rotationDirection = 1; // Clockwise
            transform.rotation = Quaternion.Euler(0f, midpointRotationY - startAngleOffset - 40, 0);
        }
        else
        {
            rotationDirection = -1; // Counterclockwise
            transform.rotation = Quaternion.Euler(0f, midpointRotationY + startAngleOffset - 40, 0f);
        }
        references.HipsAimConstraint.gameObject.SetActive(false);
        references.SpineAimContraint.gameObject.SetActive(false);
        references.RigBuilder.Build();

        StartCoroutine(DelayShooting());
    }

    private IEnumerator DelayShooting()
    {
        yield return new WaitForSeconds(0.1f);//da li mi ovo jos treba??
        isShooting = true;
    }

    private void Update()
    {
        if (isShooting)
            RotateAndShoot();
    }

    private void RotateAndShoot()
    {
        float rotationStep = rotationSpeed * Time.deltaTime * rotationDirection;
        transform.Rotate(0, rotationStep, 0);
        currentAngle += rotationStep;

        if (currentAngle >= shootingAngle || currentAngle <= -shootingAngle)
        {
            ResetAimConstraints();
            isShooting = false;
            doneShooting = true;
            return;
        }

        Shoot();
    }

    private void ResetAimConstraints()
    {
        references.HipsAimConstraint.gameObject.SetActive(true);
        references.SpineAimContraint.gameObject.SetActive(true);
        references.RigBuilder.Build();
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, Mathf.Infinity))
            if (hit.collider.GetComponent<CharacterController>())
            {
                isShooting = false;
                shotPlayer = true;
                doneShooting = true;
                ResetAimConstraints();
                EventManager.Instance.OnPlayerHitAction(references.GunData.GunDamage);
            }
    }

    public bool DoneShooting() => doneShooting;

    public void ResetShootingBools()
    {
        doneShooting = false;
        isShooting = false;
        ResetAimConstraints();
    }

    public bool ShotPlayer() => shotPlayer;

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            // Draw the forward direction of the shooting point in green
            Gizmos.color = Color.green;
            Gizmos.DrawRay(shootingPoint.position, shootingPoint.forward * 5f); // 5 units for visualization
        }
    }
}