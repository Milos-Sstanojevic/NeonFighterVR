using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerPickupController : MonoBehaviour
{
    private bool isComboActive;
    private GameObject queuedWeaponToDrop;
    private HandData hand;
    private GrabHandPose grabHandPose;
    private GunInteractionController gunInteractionController;
    private SwordInteractionController swordInteractionController;

    private void Awake()
    {
        grabHandPose = GetComponent<GrabHandPose>();
        gunInteractionController = GetComponent<GunInteractionController>();
        swordInteractionController = GetComponent<SwordInteractionController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnComboAttackStarted(() => isComboActive = true);
        EventManager.Instance.SubscribeToOnComboAttackFinished(ComboAttackFinished);
    }

    private void ComboAttackFinished()
    {
        isComboActive = false;
        if (!queuedWeaponToDrop)
            return;

        EventManager.Instance.OnReleaseWeapon(queuedWeaponToDrop, hand);
        // grabHandPose.UnsetHoldingAnimations();
        UnsetHoldingAnimations();
        queuedWeaponToDrop = null;
    }

    private void UnsetHoldingAnimations()
    {
        gunInteractionController.UnsetHoldingAnimation();
        swordInteractionController.UnsetHoldingAnimation();
    }

    public void SnapObjectToHand(SelectEnterEventArgs args)
    {
        GameObject weapon = args.interactableObject.transform.gameObject;
        HandController hand = args.interactorObject.transform.parent.GetComponent<HandController>();

        if (weapon == null || hand == null)
            return;

        EventManager.Instance.OnPickupWeapon(weapon, hand);
        // grabHandPose.SetHoldingAnimations(weapon);
        gunInteractionController.PickedUpGun(weapon);
        swordInteractionController.PickedUpSword(weapon);
    }

    public void ReleaseObjectFromHand(SelectExitEventArgs args)
    {
        GameObject weapon = args.interactableObject.transform.gameObject;
        hand = args.interactorObject.transform.parent.GetComponent<HandData>();

        if (weapon == null)
            return;

        weapon.transform.SetParent(transform);

        if (isComboActive)
        {
            queuedWeaponToDrop = weapon;
        }
        else
        {
            EventManager.Instance.OnReleaseWeapon(weapon, hand);
            UnsetHoldingAnimations();
        }
    }
}
