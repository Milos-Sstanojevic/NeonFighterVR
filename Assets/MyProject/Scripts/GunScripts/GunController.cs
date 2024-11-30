using PDollarGestureRecognizer;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private bool hasGunInHand;
    private GunSpinner gunSpinner;
    private Animator holdingHandAnimator;
    private GunShootingController gunShootingController;

    private bool isComboActive;

    private void Awake()
    {
        gunSpinner = GetComponent<GunSpinner>();
        gunShootingController = GetComponent<GunShootingController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnComboAttackFinished(ComboAttackFinished);
        EventManager.Instance.SubscribeToOnComboAttackStarted(ComboAttackIsActive);
        EventManager.Instance.SubscribeToOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(GunReleased);
        EventManager.Instance.SubscribeToOnDidGesture(GunGesture);
    }

    private void ComboAttackIsActive()
    {
        isComboActive = true;
    }

    private void ComboAttackFinished()
    {
        isComboActive = false;
    }

    private void GunPickedUp(GameObject weapon, HandController hand)
    {
        if (!weapon.GetComponent<GunController>())
            return;

        holdingHandAnimator = hand.GetComponent<HandData>().Animator;
        hasGunInHand = true;
    }

    private void GunReleased(GameObject weapon, HandData handData)
    {
        if (!weapon.GetComponent<GunController>())
            return;

        hasGunInHand = false;
    }

    private void GunGesture(Result result, GameObject source)
    {
        HandData sourceHand = source.GetComponent<HandData>();
        if (isComboActive)
            return;

        if (!sourceHand.WeaponInHand.GetComponent<GunController>())
            return;

        if (result.GestureClass == "R")
            gunSpinner.ReloadAnimation(sourceHand);


        if (result.GestureClass == "O")
        {
            isComboActive = true;
            holdingHandAnimator.applyRootMotion = false;
            holdingHandAnimator.SetBool("BigShot", true);
            EventManager.Instance.OnComboAttackStartedAction();
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnDidGesture(GunGesture);
        EventManager.Instance.UnsubscribeFromOnComboAttackStarted(ComboAttackIsActive);
        EventManager.Instance.UnsubscribeFromOnComboAttackFinished(ComboAttackFinished);
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(GunReleased);
    }

    public Animator HoldingHandAnimator() => holdingHandAnimator;
    public bool HasGunInHand() => hasGunInHand;
}
