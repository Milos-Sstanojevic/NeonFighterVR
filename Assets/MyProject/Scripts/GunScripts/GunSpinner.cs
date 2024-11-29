using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GunSpinner : MonoBehaviour
{
    private XRGrabInteractable xrInteractable;
    private HandData sourceHand;
    private GunShootingController gunController;

    private void Awake()
    {
        gunController = GetComponent<GunShootingController>();
        xrInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ReloadAnimation(HandData hand)
    {
        if (gunController.CurrentAmmo() == gunController.GetGunData().Ammo)
            return;

        sourceHand = hand;

        SetParametersOnHand(sourceHand.GunSpinningPivot);
        SetAnimatorParameters(sourceHand.Animator);
    }

    private void SetParametersOnHand(Transform pivotPoint)
    {
        transform.SetParent(pivotPoint);
        transform.localPosition = Vector3.zero;
        xrInteractable.trackPosition = false;
        xrInteractable.trackRotation = false;
    }

    private void SetAnimatorParameters(Animator animator)
    {
        animator.applyRootMotion = false;
        animator.enabled = true;
        animator.SetBool("Reloading", true);
    }

    public void ReloadingAnimationFinished()
    {
        xrInteractable.trackPosition = true;
        xrInteractable.trackRotation = true;

        UnsetAnimatorParameters(sourceHand.Animator);
    }

    private void UnsetAnimatorParameters(Animator animator)
    {
        animator.applyRootMotion = true;
        animator.SetBool("Reloading", false);
        animator.SetBool("ReloadingInterupted", false);
    }
}
