using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SwordInteractionController : MonoBehaviour
{
    [SerializeField] private Transform swordSocket;
    [SerializeField] private Transform swordAttachTransform;
    private SwordComboController swordController;
    private Rigidbody swordRb;
    private MeshCollider swordMeshCollider;
    private HandData handData;
    private XRDirectInteractor xrInteractor;
    private bool swordInInventory;

    private void Awake()
    {
        handData = GetComponent<HandData>();
        xrInteractor = GetComponentInChildren<XRDirectInteractor>();
    }

    public void PickedUpSword(GameObject weapon)
    {
        swordController = weapon.GetComponent<SwordComboController>();
        if (!swordController)
            return;

        swordRb = swordController.GetComponent<Rigidbody>();
        swordMeshCollider = swordController.GetComponent<MeshCollider>();

        handData.Animator.SetBool("PickedupSword", true);
        xrInteractor.attachTransform = swordAttachTransform;

        if (!swordInInventory)
            return;

        swordRb.useGravity = true;
        swordMeshCollider.enabled = true;
        swordInInventory = false;

        swordController.transform.SetParent(swordAttachTransform);
    }

    public void UnsetHoldingAnimation()
    {
        if (!swordController)
            return;

        handData.Animator.SetBool("PickedupSword", false);
        PutSwordInSocket();
    }

    private void PutSwordInSocket()
    {
        swordController.transform.SetParent(swordSocket);

        swordRb.useGravity = false;
        swordMeshCollider.enabled = false;
        swordRb.freezeRotation = true;
        swordRb.linearVelocity = Vector3.zero;

        swordController.transform.position = swordSocket.position;
        swordController.transform.rotation = swordSocket.rotation;

        swordInInventory = true;
    }
}