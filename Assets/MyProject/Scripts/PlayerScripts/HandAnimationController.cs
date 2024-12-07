using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;

    private Animator animator;
    private GunController gun;
    private SwordComboController sword;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnReloadingInteruptAction(StopReloadingAnimation);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(WeaponReleased);
    }

    private void StopReloadingAnimation()
    {
        if (animator.GetBool("Reloading") == false)
            return;
        animator.SetBool("ReloadingInterupted", true);
    }

    private void WeaponReleased(GameObject releasedWeapon, HandData hand)
    {
        gun = releasedWeapon.GetComponent<GunController>();
        sword = releasedWeapon.GetComponent<SwordComboController>();

        if (gun)
            StopReloadingAnimation();

        // if(sword)

    }

    private void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }

    public void ReloadingAnimationFinished()
    {
        if (gun)
            gun.GetComponent<GunReloadController>().ReloadingAnimationFinished();   //if interupted by release
        else
            GetComponentInChildren<GunReloadController>()?.ReloadingAnimationFinished(); //if animation really finished
    }

    public void CircleOfReloadingDone()
    {
        GetComponentInChildren<GunShootingController>()?.IncreaseCurrentAmmo();
    }

    public void GunReadyAfterReloadInterupt()
    {
        GunShootingController gunController = GetComponentInChildren<GunShootingController>();
        if (gunController)
            gunController.OnReadyToShoot();
    }

    public void StartBigShotParticle()
    {
        EventManager.Instance.OnBigShotStartedAction();
    }

    public void BigShotAnimationDone()
    {
        transform.position = Vector3.zero;
        EventManager.Instance.OnBigShotAnimationDoneAction(animator);
        EventManager.Instance.OnComboAttackFinishedAction();
    }

    public void FireBigBulletEvent()
    {
        EventManager.Instance.OnFireBigBulletAction();
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnReloadingInteruptAction(StopReloadingAnimation);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(WeaponReleased);
    }
}
