using System;
using PDollarGestureRecognizer;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager Instance { get; private set; }

    private event Action<GameObject, HandController> onPickupWeaponAction;
    private event Action<GameObject, HandData> onReleaseWeaponAction;
    private event Action<Result, GameObject> onDidGestureAction;
    private event Action<HandData> onStartedDrawingAction;
    private event Action<HandData> onFinishedDrawingAction;
    private event Action onReloadingInteruptAction;
    private event Action onAlienSMAttackHitAction;
    private event Action onAlienSMSwordDrawnAction;
    private event Action onAlienSMOutwardSlashAction;
    private event Action onAlienSMInwardSlashAction;
    private event Action onBigShotStartedAction;
    private event Action onComboAttackStartedAction;
    private event Action onComboAttackFinishedAction;
    private event Action<Animator> onBigShotAnimationDoneAction;
    private event Action onFireBigBulletAction;
    private event Action onAlienSMJumpAwayAnimationDone;
    private event Action onAlienSMSlashParticlePlay;
    private event Action onAlienSMSpecialAttackDone;
    private event Action onBreakStanceAnimationDone;
    private event Action<int> onPlayerHitAction;
    private event Action onStartSecondPhaseAction;
    private event Action onDrawGunsAnimationFinished;
    private event Action onSpawnTwoHolesAroundHead;
    private event Action onSpawningHolesAroundHeadFinished;
    private event Action onShieldBrokenAction;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnStartSecondPhaseAction()
    {
        onStartSecondPhaseAction?.Invoke();
    }

    public void SubscribeToOnStartSecondPhaseAction(Action action)
    {
        onStartSecondPhaseAction += action;
    }

    public void UnsubscribeFromOnStartSecondPhaseAction(Action action)
    {
        onStartSecondPhaseAction -= action;
    }

    public void OnPlayerHitAction(int enemy)
    {
        onPlayerHitAction?.Invoke(enemy);
    }

    public void SubscribeToOnPlayerHitAction(Action<int> action)
    {
        onPlayerHitAction += action;
    }

    public void UnsubscribeFromOnPlayerHitAction(Action<int> action)
    {
        onPlayerHitAction -= action;
    }

    public void OnBreakStanceAnimationDone()
    {
        onBreakStanceAnimationDone?.Invoke();
    }

    public void SubscribeToOnBreakStanceAnimationDone(Action action)
    {
        onBreakStanceAnimationDone += action;
    }

    public void UnsubscribeFromOnBreakStanceAnimationDone(Action action)
    {
        onBreakStanceAnimationDone -= action;
    }

    public void OnAlienSMSpecialAttackDone()
    {
        onAlienSMSpecialAttackDone?.Invoke();
    }

    public void SubscribeToOnAlienSMSpecialAttackDone(Action action)
    {
        onAlienSMSpecialAttackDone += action;
    }

    public void UnsubscribeFromOnAlienSMSpecialAttackDone(Action action)
    {
        onAlienSMSpecialAttackDone -= action;
    }

    public void OnAlienSMSlashParticlePlay()
    {
        onAlienSMSlashParticlePlay?.Invoke();
    }

    public void SubscribeToOnAlienSMSlashParticlePlay(Action action)
    {
        onAlienSMSlashParticlePlay += action;
    }


    public void UnsubscribeFromOnAlienSMSlashParticlePlay(Action action)
    {
        onAlienSMSlashParticlePlay -= action;
    }

    public void OnAlienSMJumpAwayAnimationDone()
    {
        onAlienSMJumpAwayAnimationDone?.Invoke();
    }

    public void SubscribeToOnAlienSMJumpAwayAnimationDone(Action action)
    {
        onAlienSMJumpAwayAnimationDone += action;
    }

    public void UnsubscribeFromOnAlienSMJumpAwayAnimationDone(Action action)
    {
        onAlienSMJumpAwayAnimationDone -= action;
    }

    public void OnFireBigBulletAction()
    {
        onFireBigBulletAction?.Invoke();
    }

    public void SubscribeToOnFireBigBulletAction(Action action)
    {
        onFireBigBulletAction += action;
    }

    public void UnsubscribeFromOnFireBigBulletAction(Action action)
    {
        onFireBigBulletAction -= action;
    }

    public void OnBigShotAnimationDoneAction(Animator animator)
    {
        onBigShotAnimationDoneAction?.Invoke(animator);
    }

    public void SubscribeToOnBigShotAnimationDoneAction(Action<Animator> action)
    {
        onBigShotAnimationDoneAction += action;
    }

    public void UnsubscribeFromOnBigShotAnimationDoneAction(Action<Animator> action)
    {
        onBigShotAnimationDoneAction -= action;
    }

    public void OnComboAttackFinishedAction()
    {
        onComboAttackFinishedAction?.Invoke();
    }

    public void SubscribeToOnComboAttackFinished(Action action)
    {
        onComboAttackFinishedAction += action;
    }

    public void UnsubscribeFromOnComboAttackFinished(Action action)
    {
        onComboAttackFinishedAction -= action;
    }

    public void OnComboAttackStartedAction()
    {
        onComboAttackStartedAction?.Invoke();
    }

    public void SubscribeToOnComboAttackStarted(Action action)
    {
        onComboAttackStartedAction += action;
    }

    public void UnsubscribeFromOnComboAttackStarted(Action action)
    {
        onComboAttackStartedAction -= action;
    }

    public void OnBigShotStartedAction()
    {
        onBigShotStartedAction?.Invoke();
    }

    public void SubscribeToBigShotAction(Action action)
    {
        onBigShotStartedAction += action;
    }

    public void UnsubscribeFromBigShotAction(Action action)
    {
        onBigShotStartedAction -= action;
    }

    public void OnAlienSMOutwardSlashAction()
    {
        onAlienSMOutwardSlashAction?.Invoke();
    }

    public void SubscribeToOnAlienSMOutwardSlashDone(Action action)
    {
        onAlienSMOutwardSlashAction += action;
    }

    public void UnsubscribeFromOnAlienSMOutwardSlashDone(Action action)
    {
        onAlienSMOutwardSlashAction -= action;
    }

    public void OnAlienSMInwardSlashAction()
    {
        onAlienSMInwardSlashAction?.Invoke();
    }

    public void SubscribeToOnAlienSMInwardSlashDone(Action action)
    {
        onAlienSMInwardSlashAction += action;
    }

    public void UnsubscribeFromOnAlienSMInwardSlashDone(Action action)
    {
        onAlienSMInwardSlashAction -= action;
    }

    public void OnAlienSMSwordDrawnAction()
    {
        onAlienSMSwordDrawnAction?.Invoke();
    }

    public void SubscribeToOnAlienSMSwordDrawnAction(Action action)
    {
        onAlienSMSwordDrawnAction += action;
    }

    public void UnsubscribeFromOnAlienSMSwordDrawnAction(Action action)
    {
        onAlienSMSwordDrawnAction -= action;
    }

    public void OnAlienSMAttackHitAction()
    {
        onAlienSMAttackHitAction?.Invoke();
    }

    public void SubscribeToOnAlienSMAttackHitAction(Action action)
    {
        onAlienSMAttackHitAction += action;
    }

    public void UnsubscribeFromOnAlienSMAttackHitAction(Action action)
    {
        onAlienSMAttackHitAction -= action;
    }

    public void OnReloadingInteruptAction()
    {
        onReloadingInteruptAction?.Invoke();
    }

    public void SubscribeToOnReloadingInteruptAction(Action action)
    {
        onReloadingInteruptAction += action;
    }

    public void UnsubscribeFromOnReloadingInteruptAction(Action action)
    {
        onReloadingInteruptAction -= action;
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

    public void OnReleaseWeapon(GameObject weapon, HandData hand)
    {
        onReleaseWeaponAction?.Invoke(weapon, hand);
    }

    public void SubscribeToOnReleaseWeaponAction(Action<GameObject, HandData> action)
    {
        onReleaseWeaponAction += action;
    }

    public void UnsubscribeFromOnReleaseWeaponAction(Action<GameObject, HandData> action)
    {
        onReleaseWeaponAction -= action;
    }

    public void OnDrawGunsAnimationFinished()
    {
        onDrawGunsAnimationFinished?.Invoke();
    }

    public void SubscribeToOnDrawGunsAnimationFinished(Action action)
    {
        onDrawGunsAnimationFinished += action;
    }

    public void UnsubscribeFromOnDrawGunsAnimationFinished(Action action)
    {
        onDrawGunsAnimationFinished -= action;
    }

    public void OnSpawnTwoHolesAroundHead()
    {
        onSpawnTwoHolesAroundHead?.Invoke();
    }

    public void SubscribeToOnSpawnTwoHolesAroundHead(Action action)
    {
        onSpawnTwoHolesAroundHead += action;
    }

    public void UnsubscribeFromOnSpawnTwoHolesAroundHead(Action action)
    {
        onSpawnTwoHolesAroundHead -= action;
    }

    public void OnSpawningHolesAroundHeadFinished()
    {
        onSpawningHolesAroundHeadFinished?.Invoke();
    }

    public void SubscribeToOnSpawningHolesAroundHeadFinished(Action action)
    {
        onSpawningHolesAroundHeadFinished += action;
    }

    public void UnsubscribeFromOnSpawningHolesAroundHeadFinished(Action action)
    {
        onSpawningHolesAroundHeadFinished -= action;
    }

    public void OnShieldBrokenAction()
    {
        onShieldBrokenAction?.Invoke();
    }

    public void SubscribeToOnShieldBrokenAction(Action action)
    {
        onShieldBrokenAction += action;
    }

    public void UnsubscribeFromOnShieldBrokenAction(Action action)
    {
        onShieldBrokenAction -= action;
    }
}
