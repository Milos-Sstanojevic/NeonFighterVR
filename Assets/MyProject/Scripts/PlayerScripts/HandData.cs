using UnityEngine;
using UnityEngine.InputSystem;

public class HandData : MonoBehaviour
{
	public enum HandModelType { Left, Right }

	public HandModelType handModelType;
	public Transform Root;
	public Animator Animator;
	public Transform[] FingerBones;

	[SerializeField] private InputActionReference drawInputActionReferenceKeyboard;
	[SerializeField] private InputActionReference drawInputActionReferenceVR;
	[SerializeField] private float inputThreshold = 0.1f;

	public GameObject WeaponInHand { get; private set; }
	public bool HasWeaponInHand { get; private set; }

	private void OnEnable()
	{
		EventManager.Instance.SubscribeToOnPickupWeaponAction(PickedWeapon);
		EventManager.Instance.SubscribeToOnReleaseWeaponAction(ReleasedWeapon);

		drawInputActionReferenceKeyboard.action.Enable();
		drawInputActionReferenceVR.action.Enable();

		drawInputActionReferenceKeyboard.action.performed += DrawOnScreen;
		drawInputActionReferenceVR.action.performed += DrawOnScreen;

		drawInputActionReferenceKeyboard.action.canceled += FinishedDrawing;
		drawInputActionReferenceVR.action.canceled += FinishedDrawing;
	}

	private void PickedWeapon(GameObject weapon, HandData hand)
	{
		if (hand != this)
			return;

		HasWeaponInHand = true;
		WeaponInHand = weapon;
	}

	private void ReleasedWeapon(GameObject weapon)
	{
		if (WeaponInHand = weapon)
		{
			HasWeaponInHand = false;
			WeaponInHand = null;
		}
	}

	private void DrawOnScreen(InputAction.CallbackContext callback)
	{
		if (callback.ReadValue<float>() > inputThreshold)
			EventManager.Instance.OnStartedDrawing(this);
	}

	private void FinishedDrawing(InputAction.CallbackContext callback)
	{
		EventManager.Instance.OnFinishedDrawing(this);
	}

	private void OnDisable()
	{
		EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(PickedWeapon);
		EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(ReleasedWeapon);

		drawInputActionReferenceKeyboard.action.Disable();
		drawInputActionReferenceVR.action.Disable();

		drawInputActionReferenceKeyboard.action.performed -= DrawOnScreen;
		drawInputActionReferenceVR.action.performed -= DrawOnScreen;

		drawInputActionReferenceKeyboard.action.canceled -= FinishedDrawing;
		drawInputActionReferenceVR.action.canceled -= FinishedDrawing;
	}
}
