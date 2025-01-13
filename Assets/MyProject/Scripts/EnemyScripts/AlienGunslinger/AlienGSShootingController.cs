using System.Collections;
using UnityEngine;

public class AlienGSShootingController : MonoBehaviour
{
    [SerializeField] private float shootingTime = 2f;
    [SerializeField] private Transform shootingPoint;
    private AlienGunslingerController alienGunslingerController;
    private ShieldController shieldController;
    private AlienGunslingerReferences references;
    private bool isShooting = false;
    private bool shouldMove = false;
    private int punishShootingDamage = 30;
    private Vector3 direction;
    private float nextFireRate;
    private bool punishOver;

    private void Start()
    {
        shieldController = GetComponentInChildren<ShieldController>();
        references = GetComponent<AlienGunslingerReferences>();
        alienGunslingerController = GetComponent<AlienGunslingerController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnChanceToSteerBulletAway(SteerBulletAway);
    }

    public void StunShoot()
    {
        //stun player and shoot at him for 1 second
        shouldMove = false;
        EventManager.Instance.OnPlayerHitAction(punishShootingDamage);
        isShooting = true;
        punishOver = false;
        StartCoroutine(StunShootCoroutine());
    }

    private IEnumerator StunShootCoroutine()
    {
        yield return new WaitForSeconds(5f);
        isShooting = false;
        punishOver = true;
    }

    public void SetupShooting()
    {
        direction = DirectionToMove();
        isShooting = true;
        shouldMove = true;

        StartCoroutine(UnsetupShooting());
    }

    private IEnumerator UnsetupShooting()
    {
        yield return new WaitForSeconds(shootingTime);
        isShooting = false;
        shouldMove = false;
    }

    private void Update()
    {
        // if (isShooting && Time.time >= nextFireRate)
        // {
        //     Shoot();
        //     nextFireRate = Time.time + references.GunData.FireRate;
        // }

        if (!shouldMove) return;

        // float speed = references.SideWalkSpeed;
        // transform.localPosition += direction * speed * Time.deltaTime;
    }

    public Vector3 DirectionToMove()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < 0.5f)
            return Vector3.right;
        else
            return Vector3.left;
    }

    // public void Shoot()
    // {
    //     RaycastHit hit;
    //     if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, Mathf.Infinity))
    //         if (hit.collider.GetComponent<CharacterController>())
    //             EventManager.Instance.OnPlayerHitAction(references.GunData.GunDamage);
    // }

    private void SteerBulletAway(ref Vector3 targetPoint, Vector3 currentPosition)
    {
        if (alienGunslingerController.GetCurrentState() != typeof(AGS_State_SideWalkAndShoot))
            return;

        shieldController.BulletSteered();

        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f),
            Random.Range(0.8f, 1f)
        ).normalized;

        float steeringStrength = Random.Range(10f, 20f);
        Vector3 steeringDirection = randomDirection * steeringStrength;

        targetPoint = currentPosition + steeringDirection;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnChanceToSteerBulletAway(SteerBulletAway);
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(shootingPoint.position, shootingPoint.forward * 40f);
        }
    }
}