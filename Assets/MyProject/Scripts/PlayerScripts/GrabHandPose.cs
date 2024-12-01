using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


//SPLIT INTO TWO OR THREE COMPONENTS

public class GrabHandPose : MonoBehaviour
{
	[SerializeField] private Transform swordSocket;
	[SerializeField] private Transform gunSocket;
	[SerializeField] private Transform swordAttachTransform;
	[SerializeField] private Transform gunAttachTransform;
	[SerializeField] private XRDirectInteractor xrInteractor;
	private HandData handData;
	private GunController gunController;
	private SwordComboController swordController;
	private bool swordInInventory;
	private bool gunInInventory;
	private Rigidbody swordRb;
	private MeshCollider swordMeshCollider;

	private void Awake()
	{
		handData = GetComponent<HandData>();
	}

	public void SetHoldingAnimations(GameObject weapon)
	{
		gunController = weapon.GetComponent<GunController>();
		swordController = weapon.GetComponent<SwordComboController>();

		PickedUpGun();
		PickedupSword();

		if (swordInInventory)
		{
			swordRb.useGravity = true;
			swordMeshCollider.enabled = true;
			swordInInventory = false;

			swordController.transform.SetParent(swordAttachTransform);
		}

		if (gunInInventory)
		{
			gunInInventory = false;
			gunController.transform.SetParent(gunAttachTransform);
		}
	}

	private void PickedUpGun()
	{
		if (!gunController)
			return;

		handData.Animator.SetBool("PickedupGun", true);
		gunController.transform.SetParent(gunAttachTransform);
		xrInteractor.attachTransform = gunAttachTransform;
	}

	private void PickedupSword()
	{
		if (!swordController)
			return;

		swordRb = swordController.GetComponent<Rigidbody>();
		swordMeshCollider = swordController.GetComponent<MeshCollider>();

		handData.Animator.SetBool("PickedupSword", true);
		xrInteractor.attachTransform = swordAttachTransform;
	}

	public void UnsetHoldingAnimations()
	{
		if (gunController)
		{
			handData.Animator.SetBool("PickedupGun", false);
			PutGunInSocket();
		}

		if (swordController)
		{
			handData.Animator.SetBool("PickedupSword", false);
			PutSwordInSocket();
		}
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

	private void PutGunInSocket()
	{
		gunController.transform.SetParent(gunSocket);
		gunController.transform.position = gunSocket.position;
		gunController.transform.rotation = gunSocket.rotation;

		gunInInventory = true;
	}
}
