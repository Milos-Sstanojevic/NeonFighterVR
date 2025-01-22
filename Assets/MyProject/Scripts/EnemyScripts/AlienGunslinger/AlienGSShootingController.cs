using System.Collections;
using UnityEngine;

public class AlienGSShootingController : MonoBehaviour
{
    [SerializeField] private float shootingTime = 2f;
    [SerializeField] private Transform shootingPointPunish;
    [SerializeField] private Transform shootingPointDownUp;
    private AlienGunslingerController alienGunslingerController;
    private ShieldController shieldController;
    private AlienGunslingerReferences references;
    private int punishShootingDamage = 30;
    private float nextFireRate;
    private bool isPunishing;
    private bool punishOver;
    private bool doneDefending;
    private bool isDownUpShooting;

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

    //stun shoot needs more work, player should not be able to move while being punished, solve by making another event
    public void StunShoot()
    {
        //stun player and shoot at him for 1 second
        EventManager.Instance.OnPlayerHitAction(punishShootingDamage);
        isPunishing = true;
        punishOver = false;
        StartCoroutine(StunShootCoroutine());
    }

    private IEnumerator StunShootCoroutine()
    {
        yield return new WaitForSeconds(5f);
        isPunishing = false;
        punishOver = true;
    }

    public void SetupDefendingFromShooting()
    {
        doneDefending = false;
        StartCoroutine(UnsetupShooting());
    }

    private IEnumerator UnsetupShooting()
    {
        yield return new WaitForSeconds(shootingTime);
        doneDefending = true;
    }

    private void Update()
    {
        if (isPunishing && Time.time >= nextFireRate)
        {
            Shoot(shootingPointPunish);
            nextFireRate = Time.time + references.GunData.FireRate;
        }

        if (isDownUpShooting && Time.time >= nextFireRate)
        {
            Shoot(shootingPointDownUp);
            nextFireRate = Time.time + references.GunData.FireRate;
        }
    }

    public void Shoot(Transform shootingPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, Mathf.Infinity))
            if (hit.collider.GetComponent<CharacterController>())
                EventManager.Instance.OnPlayerHitAction(references.GunData.GunDamage);
    }

    private void SteerBulletAway(ref Vector3 targetPoint, Vector3 currentPosition)
    {
        if (alienGunslingerController.GetCurrentState() != typeof(AGS_State_DefendingState))
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

    public void StartDownUpShoot()
    {
        isDownUpShooting = true;
    }

    public void StopDownUpShooting()
    {
        isDownUpShooting = false;
    }

    public bool IsPunishOver() => punishOver;
    public bool IsDoneDefending() => doneDefending;

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnChanceToSteerBulletAway(SteerBulletAway);
    }

    private void OnDrawGizmos()
    {
        if (shootingPointPunish != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(shootingPointPunish.position, shootingPointPunish.forward * 40f);
        }

        if (shootingPointDownUp != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(shootingPointDownUp.position, shootingPointDownUp.forward * 40f);
        }
    }
}