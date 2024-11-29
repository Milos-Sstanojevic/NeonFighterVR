using PDollarGestureRecognizer;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Animator XR;
    private bool hasGunInHand;
    private GunSpinner gunSpinner;
    private Animator holdingHandAnimator;
    private GunShootingController gunShootingController;

    private void Awake()
    {
        gunSpinner = GetComponent<GunSpinner>();
        gunShootingController = GetComponent<GunShootingController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(GunReleased);
        EventManager.Instance.SubscribeToOnDidGesture(GunGesture);
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

        if (result.GestureClass == "R")
            gunSpinner.ReloadAnimation(sourceHand);
        if (result.GestureClass == "O")
            XR.Play("BigShotAnimation");


    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(GunReleased);
    }

    public Animator HoldingHandAnimator() => holdingHandAnimator;
    public bool HasGunInHand() => hasGunInHand;
}
