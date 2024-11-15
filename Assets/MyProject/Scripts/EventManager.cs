using System;
using PDollarGestureRecognizer;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private event Action<GameObject, HandController> onPickupWeaponAction;
    private event Action<GameObject> onReleaseWeaponAction;
    private event Action<Result, GameObject> onDidGestureAction;
    private event Action<HandData> onStartedDrawingAction;
    private event Action<HandData> onFinishedDrawingAction;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnStartedDrawing(HandData hand)
    {
        onStartedDrawingAction?.Invoke(hand);
    }

    public void SubscribeToOnStartedDrawingAction(Action<HandData> action)
    {
        onStartedDrawingAction += action;
    }

    public void UnsubscribeFromOnStartedDrawingAction(Action<HandData> action)
    {
        onStartedDrawingAction -= action;
    }

    public void OnFinishedDrawing(HandData hand)
    {
        onFinishedDrawingAction?.Invoke(hand);
    }

    public void SubscribeToOnFinishedDrawingAction(Action<HandData> action)
    {
        onFinishedDrawingAction += action;
    }

    public void UnsubscribeFromOnFinishedDrawingAction(Action<HandData> action)
    {
        onFinishedDrawingAction -= action;
    }

    public void OnDidGesture(Result result, GameObject weapon)
    {
        onDidGestureAction?.Invoke(result, weapon);
    }

    public void SubscribeToOnDidGesture(Action<Result, GameObject> action)
    {
        onDidGestureAction += action;
    }

    public void UnsubscribeFromOnDidGesture(Action<Result, GameObject> action)
    {
        onDidGestureAction -= action;
    }

    public void OnPickupWeapon(GameObject weapon, HandController hand)
    {
        onPickupWeaponAction?.Invoke(weapon, hand);
    }

    public void SubscribeToOnPickupWeaponAction(Action<GameObject, HandController> action)
    {
        onPickupWeaponAction += action;
    }

    public void UnsubscribeFromOnPickupWeaponAction(Action<GameObject, HandController> action)
    {
        onPickupWeaponAction -= action;
    }

    public void OnReleaseWeapon(GameObject weapon)
    {
        onReleaseWeaponAction?.Invoke(weapon);
    }

    public void SubscribeToOnReleaseWeaponAction(Action<GameObject> action)
    {
        onReleaseWeaponAction += action;
    }

    public void UnsubscribeFromOnReleaseWeaponAction(Action<GameObject> action)
    {
        onReleaseWeaponAction -= action;
    }
}
