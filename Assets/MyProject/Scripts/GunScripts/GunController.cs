using System.Collections;
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

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(GunReleased);

        inputActionReferenceKeyboard.action.Enable();
        inputActionReferenceKeyboard.action.performed += FireBullet;

        inputActionReferenceVR.action.Enable();
        inputActionReferenceVR.action.performed += FireBullet;
    }

    private void GunPickedUp(GameObject weapon, HandData hand)
    {
        if (weapon.GetComponent<GunController>())
            hasGunInHand = true;
    }

    private void GunReleased(GameObject weapon)
    {
        if (weapon.GetComponent<GunController>())
            hasGunInHand = false;
    }

    private void FireBullet(InputAction.CallbackContext callback)
    {
        if (Time.time < nextFire || !hasGunInHand || currentAmmo == 0)
            return;

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

    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        currentAmmo = gunData.Ammo;
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
}
