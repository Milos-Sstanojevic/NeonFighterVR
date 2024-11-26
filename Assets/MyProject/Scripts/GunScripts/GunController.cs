using System.Collections;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private InputActionReference inputActionReferenceKeyboard;
    [SerializeField] private InputActionReference inputActionReferenceVR;
    [SerializeField] private Transform shootingPoint;

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private float nextFire;
    private int currentAmmo;
    private bool hasGunInHand;
    private GunSpinner gunSpinner;
    private Animator holdingHandAnimator;
    private bool isFiringPending;

    private void Awake()
    {
        gunSpinner = GetComponent<GunSpinner>();
        laserLine = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        currentAmmo = gunData.Ammo;
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(GunReleased);
        EventManager.Instance.SubscribeToOnDidGesture(GunGesture);

        inputActionReferenceKeyboard.action.Enable();
        inputActionReferenceKeyboard.action.performed += FireBullet;

        inputActionReferenceVR.action.Enable();
        inputActionReferenceVR.action.performed += FireBullet;
    }

    private void GunPickedUp(GameObject weapon, HandController hand)
    {
        if (!weapon.GetComponent<GunController>())
            return;

        holdingHandAnimator = hand.GetComponent<HandData>().Animator;
        hasGunInHand = true;
    }

    private void GunReleased(GameObject weapon)
    {
        if (!weapon.GetComponent<GunController>())
            return;

        hasGunInHand = false;

    }

    private void GunGesture(Result result, GameObject source)
    {
        HandData sourceHand = source.GetComponent<HandData>();

        if (!sourceHand.WeaponInHand.GetComponent<GunController>())
            return;

        if (result.GestureClass == "O")
            gunSpinner.ReloadAnimation(sourceHand);
    }

    private void FireBullet(InputAction.CallbackContext callback)
    {
        if (Time.time < nextFire || !hasGunInHand || currentAmmo == 0)
            return;

        if (holdingHandAnimator != null && holdingHandAnimator.GetBool("Reloading"))
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
        StartCoroutine(ShotEffect());

        RaycastHit hit;
        int positionIndex = 0;
        laserLine.SetPosition(positionIndex++, shootingPoint.position);

        if (Physics.Raycast(shootingPoint.position, transform.forward, out hit, gunData.WeaponRange))
            laserLine.SetPosition(positionIndex++, hit.point);
        else
            laserLine.SetPosition(positionIndex++, shootingPoint.position + (transform.forward * gunData.WeaponRange));
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
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
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(GunReleased);

        inputActionReferenceKeyboard.action.Disable();
        inputActionReferenceKeyboard.action.performed -= FireBullet;

        inputActionReferenceVR.action.Disable();
        inputActionReferenceVR.action.performed -= FireBullet;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public int CurrentAmmo() => currentAmmo;
    public GunData GetGunData() => gunData;
    public void IncreaseCurrentAmmo()
    {
        if (currentAmmo < gunData.Ammo)
            currentAmmo++;
    }
}
