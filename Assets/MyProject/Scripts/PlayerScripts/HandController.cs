using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private InputActionReference drawInputActionReferenceKeyboard;
    [SerializeField] private InputActionReference drawInputActionReferenceVR;
    [SerializeField] private float inputThreshold = 0.1f;

    private HandData handData;

    private void Start()
    {
        handData = GetComponent<HandData>();
    }

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

    private void PickedWeapon(GameObject weapon, HandController hand)
    {
        if (hand != this)
            return;

        handData.HasWeaponInHand = true;
        handData.WeaponInHand = weapon;
    }

    private void ReleasedWeapon(GameObject weapon)
    {
        if (handData.WeaponInHand == weapon)
        {
            handData.HasWeaponInHand = false;
            handData.WeaponInHand = null;
        }
    }

    private void DrawOnScreen(InputAction.CallbackContext callback)
    {
        if (callback.ReadValue<float>() > inputThreshold)
            EventManager.Instance.OnStartedDrawing(GetComponent<HandData>());
    }

    private void FinishedDrawing(InputAction.CallbackContext callback)
    {
        EventManager.Instance.OnFinishedDrawing(GetComponent<HandData>());
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
