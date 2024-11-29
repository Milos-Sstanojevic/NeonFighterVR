using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float dashTime;
	[SerializeField] private float dashCooldown;
	[SerializeField] private float dashForce;
	[SerializeField] private float jumpHeight;
	[SerializeField] private float negativeGravityMultiplier;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private InputActionProperty jumpButtonVR;
	[SerializeField] private InputActionProperty jumpButtonKeyboard;
	[SerializeField] private InputActionProperty dashButtonVR;
	[SerializeField] private InputActionProperty dashButtonKeyboard;
	[SerializeField] private LayerMask groundLayer;
	private Vector3 movement;
	private bool isDashing = false;
	private bool canDash = true;


	private void OnEnable()
	{
		SubscribeForDashingKeyboard();
		SubscribeForDashingVR();
	}

	private void SubscribeForDashingKeyboard()
	{
		dashButtonKeyboard.action.started += StartDash;
		dashButtonKeyboard.action.canceled += StopDash;
		dashButtonKeyboard.action.Enable();
	}

	private void SubscribeForDashingVR()
	{
		dashButtonVR.action.started += StartDash;
		dashButtonVR.action.canceled += StopDash;
		dashButtonVR.action.Enable();
	}

	private void StartDash(InputAction.CallbackContext cb)
	{
		if (canDash && !isDashing)
			StartCoroutine(Dash());
	}

	private void StopDash(InputAction.CallbackContext cb)
	{
		if (!isDashing)
			return;

		StopCoroutine(Dash());
		isDashing = false;
	}

	private IEnumerator Dash()
	{
		isDashing = true;
		canDash = false;
		float startTime = Time.time;

		while (Time.time < startTime + dashTime)
		{
			characterController.Move(dashForce * Time.deltaTime * Camera.main.transform.forward);
			yield return null;
		}

		isDashing = false;
		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}

	private void Update()
	{
		Jump();
	}

	private void Jump()
	{
		if ((jumpButtonVR.action.WasPressedThisFrame() || jumpButtonKeyboard.action.WasPressedThisFrame()) && IsGrounded())
			movement.y = Mathf.Sqrt(jumpHeight * negativeGravityMultiplier * Physics.gravity.y);

		movement.y += Physics.gravity.y * Time.deltaTime;

		characterController.Move(movement * Time.deltaTime);
	}

	private bool IsGrounded() => Physics.CheckSphere(characterController.transform.position, 0.5f, groundLayer);

	public void ShootBigShot()
	{
		EventManager.Instance.OnBigShotStartedAction();
	}

	private void OnDisable()
	{
		UnsubscribeForDashingKeyboard();
		UnsubscribeForDashingVR();
	}

	private void UnsubscribeForDashingKeyboard()
	{
		dashButtonKeyboard.action.started -= StartDash;
		dashButtonKeyboard.action.canceled -= StopDash;
		dashButtonKeyboard.action.Disable();
	}

	private void UnsubscribeForDashingVR()
	{
		dashButtonVR.action.started -= StartDash;
		dashButtonVR.action.canceled -= StopDash;
		dashButtonVR.action.Disable();
	}
}
