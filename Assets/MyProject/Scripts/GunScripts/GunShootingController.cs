using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShootingController : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private InputActionReference inputActionReferenceKeyboard;
    [SerializeField] private InputActionReference inputActionReferenceVR;
    [SerializeField] private List<Transform> shootingPoints;
    [SerializeField] private List<ParticleSystem> bullets;
    [SerializeField] private List<ParticleSystem> muzzles;
    [SerializeField] private float lineTime;
    private GunController gunController;
    private float nextFire;
    private bool isFiringPending;
    private int currentAmmo;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    private void Awake()
    {
        gunController = GetComponent<GunController>();
    }

    private void Start()
    {
        currentAmmo = gunData.Ammo;
        foreach (ParticleSystem bullet in bullets)
            lineRenderers.Add(bullet.GetComponentInChildren<LineRenderer>(true));
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnFireBigBulletAction(FireBigBullet);
        inputActionReferenceKeyboard.action.Enable();
        inputActionReferenceKeyboard.action.performed += FireBullet;

        inputActionReferenceVR.action.Enable();
        inputActionReferenceVR.action.performed += FireBullet;
    }

    private void FireBigBullet()
    {
        currentAmmo = 0;
    }

    private void FireBullet(InputAction.CallbackContext callback)
    {
        if (Time.time < nextFire || !gunController.HasGunInHand() || currentAmmo == 0)
            return;

        if (gunController.HoldingHandAnimator() != null && gunController.HoldingHandAnimator().GetBool("Reloading"))
        {
            isFiringPending = true;
            EventManager.Instance.OnReloadingInteruptAction();
            return;
        }

        ExecuteFire();
    }

    public void ExecuteFire()
    {
        currentAmmo--;

        nextFire = Time.time + gunData.FireRate;

        RaycastHit hit;
        Vector3 targetPosition;
        if (Physics.Raycast(shootingPoints[currentAmmo].position, shootingPoints[currentAmmo].forward, out hit, gunData.WeaponRange))
            targetPosition = hit.point;
        else
            targetPosition = shootingPoints[currentAmmo].position + shootingPoints[currentAmmo].forward * gunData.BulletSpeed;

        DealDamageToTarget(hit);

        StartCoroutine(MoveBullet(currentAmmo, targetPosition));
    }

    private void DealDamageToTarget(RaycastHit hit)
    {
        EnemyDamageController alien = hit.rigidbody?.GetComponent<EnemyDamageController>();
        if (!alien)
            return;

        alien.TakeDamage(gunData.GunDamage);
    }

    private IEnumerator MoveBullet(int barrelIndex, Vector3 targetPoint)
    {
        muzzles[barrelIndex].Play();
        bullets[barrelIndex].gameObject.SetActive(true);
        bullets[barrelIndex].Play();

        Vector3 startPosition = shootingPoints[barrelIndex].transform.position;
        float distance = Vector3.Distance(startPosition, targetPoint);
        float travelTime = distance / gunData.BulletSpeed;
        float elapsedTime = 0f;
        bullets[barrelIndex].gameObject.transform.SetParent(null);

        LineRenderer line = lineRenderers[barrelIndex];
        line.positionCount = 2;
        line.SetPosition(0, startPosition);
        line.SetPosition(1, startPosition);

        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travelTime;
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPoint, t);
            bullets[barrelIndex].transform.position = currentPosition;

            line.SetPosition(1, currentPosition);

            yield return null;
        }

        yield return new WaitForSeconds(lineTime);

        bullets[barrelIndex].gameObject.SetActive(false);
        bullets[barrelIndex].transform.position = targetPoint;
        bullets[barrelIndex].gameObject.transform.SetParent(shootingPoints[barrelIndex]);
        bullets[barrelIndex].transform.localPosition = Vector3.zero;
        bullets[barrelIndex].transform.localEulerAngles = new Vector3(-90, 0, 0);
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);


    }

    public void OnReadyToShoot()
    {
        if (!isFiringPending)
            return;

        ExecuteFire();
        isFiringPending = false;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnFireBigBulletAction(FireBigBullet);
        inputActionReferenceKeyboard.action.Disable();
        inputActionReferenceKeyboard.action.performed -= FireBullet;

        inputActionReferenceVR.action.Disable();
        inputActionReferenceVR.action.performed -= FireBullet;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public GunData GetGunData() => gunData;
    public int CurrentAmmo() => currentAmmo;
    public void IncreaseCurrentAmmo()
    {
        if (currentAmmo < gunData.Ammo)
            currentAmmo++;
    }
}