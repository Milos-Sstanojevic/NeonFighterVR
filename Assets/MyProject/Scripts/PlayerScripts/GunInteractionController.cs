using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GunInteractionController : MonoBehaviour
{
    [SerializeField] private Transform gunSocket;
    [SerializeField] private Transform gunAttachTransform;
    private GunController gunController;
    private HandData handData;
    private bool gunInInventory;
    private XRDirectInteractor xrInteractor;

    private void Awake()
    {
        handData = GetComponent<HandData>();
        xrInteractor = GetComponentInChildren<XRDirectInteractor>();
    }

    public void PickedUpGun(GameObject gun)
    {
        gunController = gun.GetComponent<GunController>();

        if (!gunController)
            return;

        handData.Animator.SetBool("PickedupGun", true);
        gunController.transform.SetParent(gunAttachTransform);
        xrInteractor.attachTransform = gunAttachTransform;

        if (!gunInInventory)
            return;

        gunInInventory = false;
        gunController.transform.SetParent(gunAttachTransform);
    }

    public void UnsetHoldingAnimation()
    {
        if (!gunController)
            return;

        handData.Animator.SetBool("PickedupGun", false);
        PutGunInSocket();
    }

    private void PutGunInSocket()
    {
        gunController.transform.SetParent(gunSocket);
        gunController.transform.position = gunSocket.position;
        gunController.transform.rotation = gunSocket.rotation;

        gunInInventory = true;
    }
}