using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class GrabHandPose : MonoBehaviour
{
	[SerializeField] private Transform swordAttachTransformLeft;
	[SerializeField] private Transform swordAttachTransformRight;
	[SerializeField] private Transform gunAttachTransformLeft;
	[SerializeField] private Transform gunAttachTransformRight;
	[SerializeField] private Transform swordSocket;
	[SerializeField] private Transform gunSocket;

	private bool swordInInventory;
	private bool gunInInventory;
	private Rigidbody swordRb;
	private MeshCollider swordMeshCollider;
	private GunController gunController;
	private SwordComboController swordController;

	private void Start()
	{
		XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
		swordRb = GetComponent<Rigidbody>();
		swordMeshCollider = GetComponent<MeshCollider>();

		gunController = transform.GetComponent<GunController>();
		swordController = transform.GetComponent<SwordComboController>();

		grabInteractable.selectEntered.AddListener(SetHoldingAnimations);
		grabInteractable.selectExited.AddListener(UnsetHoldingAnimations);
	}

	private void SetHoldingAnimations(BaseInteractionEventArgs arg)
	{
		if (!(arg.interactorObject is XRDirectInteractor))
			return;

		HandData handData = arg.interactorObject.transform.parent.GetComponent<HandData>();

		PickedUpGun(handData);
		PickedupSword(handData);

		if (swordInInventory)
		{
			swordRb.useGravity = true;
			swordMeshCollider.enabled = true;
			swordInInventory = false;

			transform.SetParent(null);
		}

		if (gunInInventory)
		{
			gunInInventory = false;
			transform.SetParent(null);
		}
	}

	private void PickedUpGun(HandData handData)
	{
		if (!gunController)
			return;

		handData.Animator.SetBool("PickedupGun", true);
		if (handData.handModelType == HandData.HandModelType.Left)
			handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = gunAttachTransformLeft;
		else
			handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = gunAttachTransformRight;

	}

	private void PickedupSword(HandData handData)
	{
		if (!swordController)
			return;

		handData.Animator.SetBool("PickedupSword", true);
		if (handData.handModelType == HandData.HandModelType.Left)
			handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = swordAttachTransformLeft;
		else
			handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = swordAttachTransformRight;
	}

	private void UnsetHoldingAnimations(BaseInteractionEventArgs arg)
	{
		if (!(arg.interactorObject is XRDirectInteractor))
			return;

		HandData handData = arg.interactorObject.transform.parent.GetComponent<HandData>();

		if (gunController)
			handData.Animator.SetBool("PickedupGun", false);

		if (swordController)
			handData.Animator.SetBool("PickedupSword", false);

		if (swordController)
			PutSwordInSocket();
		if (gunController)
			PutGunInSocket();
	}

	private void PutSwordInSocket()
	{
		transform.SetParent(swordSocket);

		swordRb.useGravity = false;
		swordMeshCollider.enabled = false;
		swordRb.freezeRotation = true;
		swordRb.linearVelocity = Vector3.zero;

		transform.position = swordSocket.position;
		transform.rotation = swordSocket.rotation;

		swordInInventory = true;
	}

	private void PutGunInSocket()
	{
		transform.SetParent(gunSocket);
		transform.position = gunSocket.position;
		transform.rotation = gunSocket.rotation;

		gunInInventory = true;
	}
}
