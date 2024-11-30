using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerPickupController : MonoBehaviour
{
    private bool isComboActive;
    private GameObject queuedWeaponToDrop;
    private HandData hand;
    private GrabHandPose grabHandPose;

    private void Awake()
    {
        grabHandPose = GetComponentInChildren<GrabHandPose>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnComboAttackStarted(() => isComboActive = true);
        EventManager.Instance.SubscribeToOnComboAttackFinished(ComboAttackFinished);
    }

    private void ComboAttackFinished()
    {
        isComboActive = false;
        if (queuedWeaponToDrop)
        {
            EventManager.Instance.OnReleaseWeapon(queuedWeaponToDrop, hand);
            grabHandPose.UnsetHoldingAnimations();
            queuedWeaponToDrop = null;
        }
    }

    public void SnapObjectToHand(SelectEnterEventArgs args)
    {
        GameObject weapon = args.interactableObject.transform.gameObject;
        HandController hand = args.interactorObject.transform.parent.GetComponent<HandController>();

        if (weapon != null && hand != null)
        {
            EventManager.Instance.OnPickupWeapon(weapon, hand);
            grabHandPose.SetHoldingAnimations(weapon);
        }
    }

    public void ReleaseObjectFromHand(SelectExitEventArgs args)
    {
        GameObject weapon = args.interactableObject.transform.gameObject;
        hand = args.interactorObject.transform.parent.GetComponent<HandData>();

        if (weapon == null)
            return;

        if (isComboActive)
            queuedWeaponToDrop = weapon;
        else
        {
            EventManager.Instance.OnReleaseWeapon(weapon, hand);
            grabHandPose.UnsetHoldingAnimations();
        }
    }
}
