using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShootingController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private GunData gunData;
    [SerializeField] private InputActionReference inputActionReferenceKeyboard;
    [SerializeField] private InputActionReference inputActionReferenceVR;
    [SerializeField] private List<Transform> shootingPoints;
    [SerializeField] private List<ParticleSystem> bullets;
    [SerializeField] private List<ParticleSystem> muzzles;
    [SerializeField] private ParticleSystem bigShot;

    private GunController gunController;
    private float nextFire;
    private bool isFiringPending;
    private int currentAmmo;

    private void Awake()
    {
        gunController = GetComponent<GunController>();
    }

    private void Start()
    {
        currentAmmo = gunData.Ammo;
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToBigShot(BigShot);
        inputActionReferenceKeyboard.action.Enable();
        inputActionReferenceKeyboard.action.performed += FireBullet;

        inputActionReferenceVR.action.Enable();
        inputActionReferenceVR.action.performed += FireBullet;
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
            targetPosition = shootingPoints[currentAmmo].position + shootingPoints[currentAmmo].forward * speed;

        StartCoroutine(MoveBullet(currentAmmo, targetPosition));
    }

    private IEnumerator MoveBullet(int barrelIndex, Vector3 targetPoint)
    {
        muzzles[barrelIndex].Play();
        bullets[barrelIndex].gameObject.SetActive(true);
        bullets[barrelIndex].Play();

        Vector3 startPosition = shootingPoints[barrelIndex].transform.position;
        float distance = Vector3.Distance(startPosition, targetPoint);
        float travelTime = distance / speed;
        float elapsedTime = 0f;
        Transform parent = bullets[barrelIndex].gameObject.transform.parent;
        bullets[barrelIndex].gameObject.transform.SetParent(null);

        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travelTime;
            bullets[barrelIndex].transform.position = Vector3.Lerp(startPosition, targetPoint, t);

            yield return null;
        }

        bullets[barrelIndex].gameObject.SetActive(false);
        bullets[barrelIndex].gameObject.transform.SetParent(parent);
        bullets[barrelIndex].transform.position = targetPoint;
        bullets[barrelIndex].transform.localPosition = Vector3.zero;
        bullets[barrelIndex].transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    public void OnReadyToShoot()
    {
        if (!isFiringPending)
            return;

        ExecuteFire();
        isFiringPending = false;
    }

    public void BigShot()
    {
        bigShot.Play();
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromBigShot(BigShot);

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