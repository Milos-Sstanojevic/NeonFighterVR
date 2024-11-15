using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
	[SerializeField] private HandData rightHandPose;
	[SerializeField] private HandData leftHandPose;
	[SerializeField] private Transform swordAttachTransformLeft;
	[SerializeField] private Transform swordAttachTransformRight;
	[SerializeField] private Transform gunAttachTransformLeft;
	[SerializeField] private Transform gunAttachTransformRight;
	[SerializeField] private Transform swordSocket;
	[SerializeField] private Transform gunSocket;

	private bool swordInInventory;
	private bool gunInInventory;
	private Vector3 startingHandPosition;
	private Vector3 finalHandPosition;
	private Quaternion startingHandRotation;
	private Quaternion finalHandRotation;
	private Quaternion[] startingFingerRotations;
	private Quaternion[] finalFingerRotations;
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

		grabInteractable.selectEntered.AddListener(SetupPose);
		grabInteractable.selectExited.AddListener(UnsetPose);

		rightHandPose.gameObject.SetActive(false);
		leftHandPose.gameObject.SetActive(false);
	}

	private void SetupPose(BaseInteractionEventArgs arg)
	{
		if (!(arg.interactorObject is XRDirectInteractor))
			return;

		HandData handData = arg.interactorObject.transform.parent.GetComponent<HandData>();
		handData.Animator.enabled = false;

		if (gunController)
		{
			if (handData.handModelType == HandData.HandModelType.Left)
			{
				handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = gunAttachTransformLeft;
				SetHandDataValues(handData, leftHandPose);
			}
			else
			{
				handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = gunAttachTransformRight;
				SetHandDataValues(handData, rightHandPose);
			}
		}

		if (swordController)
		{
			if (handData.handModelType == HandData.HandModelType.Left)
			{
				handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = swordAttachTransformLeft;
				SetHandDataValues(handData, leftHandPose);
			}
			else
			{
				handData.GetComponentInChildren<XRDirectInteractor>().attachTransform = swordAttachTransformRight;
				SetHandDataValues(handData, rightHandPose);
			}
		}
		SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);

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

	private void UnsetPose(BaseInteractionEventArgs arg)
	{
		if (!(arg.interactorObject is XRDirectInteractor))
			return;

		HandData handData = arg.interactorObject.transform.parent.GetComponent<HandData>();
		handData.Animator.enabled = true;

		SetHandDataValues(handData, rightHandPose);

		SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);

		if (swordController)
			PutSwordInSocket();
		if (gunController)
			PutGunInSocket();
	}

	private void SetHandDataValues(HandData h1, HandData h2)
	{
		startingHandPosition = new Vector3(h1.Root.localPosition.x / h1.Root.localScale.x, h1.Root.localPosition.y / h1.Root.localScale.y, h1.Root.localPosition.z / h1.Root.localScale.z);
		finalHandPosition = new Vector3(h2.Root.localPosition.x / h2.Root.localScale.x, h2.Root.localPosition.y / h2.Root.localScale.y, h2.Root.localPosition.z / h2.Root.localScale.z);

		startingHandRotation = h1.Root.localRotation;
		finalHandRotation = h2.Root.localRotation;

		startingFingerRotations = new Quaternion[h1.FingerBones.Length];
		finalFingerRotations = new Quaternion[h2.FingerBones.Length];

		for (int i = 0; i < h1.FingerBones.Length; i++)
		{
			startingFingerRotations[i] = h1.FingerBones[i].localRotation;
			finalFingerRotations[i] = h2.FingerBones[i].localRotation;
		}
	}

	private void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
	{
		h.Root.localPosition = newPosition;
		h.Root.localRotation = newRotation;


		for (int i = 0; i < newBonesRotation.Length; i++)
			h.FingerBones[i].localRotation = newBonesRotation[i];
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

#if UNITY_EDITOR
	[MenuItem("Tools/Mirror Selected Right Grab Pose")]
	public static void MirrorRightPose()
	{
		Debug.Log("Mirror");
		GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
		handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
	}
#endif

	private void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
	{
		Vector3 mirroredPosition = poseUsedToMirror.Root.localPosition;
		mirroredPosition.x *= -1;

		Quaternion mirroredQuaternion = poseUsedToMirror.Root.localRotation;
		mirroredQuaternion.y *= -1;
		mirroredQuaternion.z *= -1;

		poseToMirror.Root.localPosition = mirroredPosition;
		poseToMirror.Root.localRotation = mirroredQuaternion;

		for (int i = 0; i < poseUsedToMirror.FingerBones.Length; i++)
			poseToMirror.FingerBones[i].localRotation = poseUsedToMirror.FingerBones[i].localRotation;
	}
}
