using UnityEngine;

public class AlienGSSideToSideShootController : MonoBehaviour
{
    [SerializeField] private int shootingAngle = 160;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float shootingInterval = 0.5f;
    private AlienGunslingerReferences references;
    private CharacterController characterController;
    private float currentAngle;
    private bool isShooting;
    private float nextShootTime = 0;
    private int rotationDirection = 0;

    private void Awake()
    {
        references = GetComponent<AlienGunslingerReferences>();
        characterController = references.Character;
    }

    public void StartShooting()
    {
        // Determine if the player is to the left or right of the enemy on the x-axis
        float playerRelativeX = characterController.transform.position.x - transform.position.x;

        // Set initial rotation to the side opposite the player's relative position
        float startAngleOffset = shootingAngle / 2;
        if (playerRelativeX > 0) // Player is to the right
        {
            rotationDirection = -1; // Clockwise
            transform.rotation = Quaternion.Euler(0f, 0f, -startAngleOffset);
        }
        else // Player is to the left
        {
            rotationDirection = 1; // Counterclockwise
            transform.rotation = Quaternion.Euler(0f, 0f, startAngleOffset);
        }

        currentAngle = 0f;
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
        transform.Rotate(0, 0, rotationStep);
        currentAngle += rotationStep;

        references.ShootingController.StartShooting();

        if (currentAngle >= shootingAngle)
            references.ShootingController.StopShooting();
    }


    public bool DoneShooting() => !isShooting;
}