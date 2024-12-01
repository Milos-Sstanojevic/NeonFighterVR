using PDollarGestureRecognizer;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private bool hasGunInHand;
    private GunReloadController gunSpinner;
    private Animator holdingHandAnimator;
    private GunBigShotController gunBigShotController;

    private void Awake()
    {
        gunSpinner = GetComponent<GunReloadController>();
        gunBigShotController = GetComponent<GunBigShotController>();
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

    private void GunReleased(GameObject weapon, HandData handData)
    {
        if (!weapon.GetComponent<GunController>())
            return;

        hasGunInHand = false;
    }

    private void GunGesture(Result result, GameObject source)
    {
        HandData sourceHand = source.GetComponent<HandData>();
        if (gunBigShotController.IsComboActive())
            return;

        if (!sourceHand.WeaponInHand.GetComponent<GunController>())
            return;

        if (result.GestureClass == "I")
            gunSpinner.ReloadAnimation(sourceHand);


        if (result.GestureClass == "O")
            gunBigShotController.BigShot(holdingHandAnimator);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnDidGesture(GunGesture);
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(GunPickedUp);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(GunReleased);
    }

    public Animator HoldingHandAnimator() => holdingHandAnimator;
    public bool HasGunInHand() => hasGunInHand;
}
