using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerPickupController : MonoBehaviour
{
    public void SnapObjectToHand(SelectEnterEventArgs args)
    {
        GameObject weapon = args.interactableObject.transform.gameObject;
        HandController hand = args.interactorObject.transform.parent.GetComponent<HandController>();

        if (weapon != null && hand != null)
            EventManager.Instance.OnPickupWeapon(weapon, hand);
    }

    public void ReleaseObjectFromHand(SelectExitEventArgs args)
    {
        GameObject weapon = args.interactableObject.transform.gameObject;

        if (weapon)
            EventManager.Instance.OnReleaseWeapon(weapon);
    }
}
