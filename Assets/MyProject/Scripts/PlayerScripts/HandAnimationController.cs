using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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

    //ovo treba da se prepravi (da bude ono performed a ne ovako u Update)
    private void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }

    public void ReloadingAnimationFinished()
    {
        gun?.GetComponent<GunSpinner>().ReloadingAnimationFinished();   //if interupted by release
        GetComponentInChildren<GunSpinner>()?.ReloadingAnimationFinished(); //if animation really finished
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

    public void ShootBigShot()
    {
        EventManager.Instance.OnBigShotStartedAction();
    }

    public void BigShotAnimationDone()
    {
        transform.eulerAngles = new Vector3(0, 0, 90);
        animator.applyRootMotion = true;
        animator.SetBool("BigShot", false);
        EventManager.Instance.OnComboAttackFinishedAction();
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnReloadingInteruptAction(StopReloadingAnimation);
    }
}
