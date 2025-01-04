using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GunReloadController : MonoBehaviour
{
    private XRGrabInteractable xrInteractable;
    private HandData sourceHand;
    private GunShootingController gunController;
    private Quaternion oldRotation;

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
        oldRotation = sourceHand.transform.rotation;

        SetParametersOnHand();
        SetAnimatorParameters();
    }

    private void SetParametersOnHand()
    {
        transform.SetParent(sourceHand.GunSpinningPivot);
        transform.localPosition = Vector3.zero;
        xrInteractable.trackPosition = false;
        xrInteractable.trackRotation = false;
    }

    private void SetAnimatorParameters()
    {
        sourceHand.Animator.SetBool("Reloading", true);
    }

    public void ReloadingAnimationFinished()
    {
        xrInteractable.trackPosition = true;
        xrInteractable.trackRotation = true;
        UnsetAnimatorParameters();
    }

    private void UnsetAnimatorParameters()
    {
        sourceHand.Animator.SetBool("Reloading", false);
        sourceHand.Animator.SetBool("ReloadingInterupted", false);
        StartCoroutine(ResetHandRotation());
    }

    //nije najbolje resenja, ali trazi testiranje sa vr controllerima
    private IEnumerator ResetHandRotation()
    {
        yield return new WaitForSeconds(0.3f);
        sourceHand.transform.rotation = oldRotation;
    }
}
