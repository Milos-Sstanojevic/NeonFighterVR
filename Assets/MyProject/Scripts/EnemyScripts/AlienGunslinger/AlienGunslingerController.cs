using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGunslingerController : MonoBehaviour
{
    private bool playerTriedToAttack = false;
    private bool shieldBroken = false;
    private bool shieldRecovered;
    private bool secondPhase;
    private StateMachine stateMachine;
    private AlienGunslingerReferences alienGunslingerReferences;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnShieldBrokenAction(ShieldBroken);
        EventManager.Instance.SubscribeToOnStartSecondPhaseAction(StartSecondPhase);
        EventManager.Instance.SubscribeToOnShieldHitAction(PlayerTriedToAttack);
    }

    private void ShieldBroken()
    {
        shieldBroken = true;
        shieldRecovered = false;
        StartCoroutine(RecoveringShield());
    }

    private void StartSecondPhase()
    {
        secondPhase = true;
    }

    private void PlayerTriedToAttack()
    {
        if (stateMachine.CurrentState.GetType() == typeof(AGS_State_IdleProvoking))
            playerTriedToAttack = true;
    }

    public void UnsetPlayerTriedToAttack()
    {
        playerTriedToAttack = false;
    }

    private void Awake()
    {
        secondPhase = false;
        stateMachine = new StateMachine();
        alienGunslingerReferences = GetComponent<AlienGunslingerReferences>();

        AGS_State_DrawGun drawGun = new AGS_State_DrawGun(alienGunslingerReferences);
        AGS_State_DefendingState defendingState = new AGS_State_DefendingState(alienGunslingerReferences);
        ASG_State_AroundHeadAttack aroundHeadAttack = new ASG_State_AroundHeadAttack(alienGunslingerReferences);
        AGS_State_SideToSideShoot sideToSideShoot = new AGS_State_SideToSideShoot(alienGunslingerReferences);

        AGS_State_IdleProvoking idleProvoking = new AGS_State_IdleProvoking(alienGunslingerReferences, this);
        AGS_State_BrokenShieldIdle brokenShieldIdle = new AGS_State_BrokenShieldIdle(alienGunslingerReferences);
        AGS_State_DashFromPlayer dashFromPlayer = new AGS_State_DashFromPlayer(alienGunslingerReferences);
        AGS_State_RecoverShield recoverShield = new AGS_State_RecoverShield(alienGunslingerReferences);
        AGS_State_PunishPlayer punishPlayer = new AGS_State_PunishPlayer(alienGunslingerReferences);
        AGS_State_Walk walk = new AGS_State_Walk(alienGunslingerReferences);
        AGS_State_DownUpShoot downUpShoot = new AGS_State_DownUpShoot(alienGunslingerReferences);

        AddTransition(drawGun, defendingState, () => drawGun.IsDone());

        AddTransition(aroundHeadAttack, defendingState, () => aroundHeadAttack.IsDone() && !ShouldGoToIdleProvoking() && secondPhase);
        AddTransition(aroundHeadAttack, idleProvoking, () => aroundHeadAttack.IsDone() && ShouldGoToIdleProvoking()); //da li da dozvolim da ga provocira dok mu je aktivno pucanje oko glave?
        AddTransition(aroundHeadAttack, walk, () => !secondPhase && !ShouldGoToIdleProvoking() && aroundHeadAttack.IsDone());

        AddTransition(sideToSideShoot, walk, () => alienGunslingerReferences.SideToSideShootController.DoneShooting() && !alienGunslingerReferences.SideToSideShootController.ShotPlayer() && !ShouldGoToIdleProvoking());
        AddTransition(sideToSideShoot, idleProvoking, () => alienGunslingerReferences.SideToSideShootController.DoneShooting() && !alienGunslingerReferences.SideToSideShootController.ShotPlayer() && ShouldGoToIdleProvoking());
        AddTransition(sideToSideShoot, punishPlayer, () => alienGunslingerReferences.SideToSideShootController.ShotPlayer());

        AddTransition(downUpShoot, walk, () => alienGunslingerReferences.DownUpShootController.DoneShooting() && !alienGunslingerReferences.DownUpShootController.IsShotPlayer() && !ShouldGoToIdleProvoking());
        AddTransition(downUpShoot, idleProvoking, () => alienGunslingerReferences.DownUpShootController.DoneShooting() && !alienGunslingerReferences.DownUpShootController.IsShotPlayer() && ShouldGoToIdleProvoking());
        AddTransition(downUpShoot, punishPlayer, () => alienGunslingerReferences.DownUpShootController.IsShotPlayer());

        AddTransition(defendingState, aroundHeadAttack, () => alienGunslingerReferences.ShootingController.IsDoneDefending() && GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !ShouldGoToIdleProvoking());
        AddTransition(defendingState, downUpShoot, () => alienGunslingerReferences.ShootingController.IsDoneDefending() && GetOrComputeAttackSelectionChance() == typeof(AGS_State_DownUpShoot) && !ShouldGoToIdleProvoking());
        AddTransition(defendingState, sideToSideShoot, () => alienGunslingerReferences.ShootingController.IsDoneDefending() && GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && !ShouldGoToIdleProvoking());
        AddTransition(defendingState, walk, () => alienGunslingerReferences.ShootingController.IsDoneDefending() && !ShouldGoToIdleProvoking());
        AddTransition(defendingState, idleProvoking, () => alienGunslingerReferences.ShootingController.IsDoneDefending() && ShouldGoToIdleProvoking());

        AddTransition(walk, sideToSideShoot, () => alienGunslingerReferences.WalkController.IsDoneWalking() && GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && !ShouldGoToIdleProvoking() && CanDoMultipleSpecialAttacks());
        AddTransition(walk, downUpShoot, () => alienGunslingerReferences.WalkController.IsDoneWalking() && GetOrComputeAttackSelectionChance() == typeof(AGS_State_DownUpShoot) && !ShouldGoToIdleProvoking() && CanDoMultipleSpecialAttacks());
        AddTransition(walk, aroundHeadAttack, () => alienGunslingerReferences.WalkController.IsDoneWalking() && GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !ShouldGoToIdleProvoking() && !alienGunslingerReferences.AroundHeadAttackController.HasActiveHolesAroundHead());
        AddTransition(walk, defendingState, () => alienGunslingerReferences.WalkController.IsDoneWalking() && GetOrComputeAttackSelectionChance() == typeof(AGS_State_DefendingState) && !ShouldGoToIdleProvoking());
        AddTransition(walk, idleProvoking, () => alienGunslingerReferences.WalkController.IsDoneWalking() && ShouldGoToIdleProvoking()); //da li da dozvolim da ga provocira dok ima rupe oko glave??
        AddTransition(walk, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());

        AddTransition(idleProvoking, sideToSideShoot, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && CanDoMultipleSpecialAttacks() && idleProvoking.IsDone());
        AddTransition(idleProvoking, downUpShoot, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(AGS_State_DownUpShoot) && CanDoMultipleSpecialAttacks() && idleProvoking.IsDone());
        AddTransition(idleProvoking, aroundHeadAttack, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !alienGunslingerReferences.AroundHeadAttackController.HasActiveHolesAroundHead() && idleProvoking.IsDone());
        AddTransition(idleProvoking, defendingState, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(AGS_State_DefendingState) && idleProvoking.IsDone());
        AddTransition(idleProvoking, punishPlayer, () => playerTriedToAttack); //da li da dozvolim da ga punishuje dok ima rupe oko glave??
        AddTransition(idleProvoking, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());


        AddTransition(punishPlayer, walk, () => alienGunslingerReferences.ShootingController.IsPunishOver());

        AddTransition(defendingState, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());

        AddTransition(dashFromPlayer, walk, () => alienGunslingerReferences.DashingController.GetDashDone() && !ShouldGoToIdleProvoking());
        AddTransition(dashFromPlayer, idleProvoking, () => alienGunslingerReferences.DashingController.GetDashDone() && ShouldGoToIdleProvoking());

        Any(brokenShieldIdle, () => shieldBroken);
        AddTransition(brokenShieldIdle, recoverShield, () => shieldRecovered);
        AddTransition(recoverShield, walk, () => recoverShield.IsDone());

        stateMachine.SetState(drawGun);
    }

    private void AddTransition(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);

    private bool ShouldGoToIdleProvoking() => GetOrComputeIdleOrIdleProvoking() == typeof(AGS_State_IdleProvoking);

    private bool CanDoMultipleSpecialAttacks()
    {
        if (alienGunslingerReferences.AroundHeadAttackController.HasActiveHolesAroundHead() && !secondPhase)
            return false;

        return true;
    }

    private Type GetOrComputeIdleOrIdleProvoking()
    {
        if (alienGunslingerReferences.CachedIdleOrIdleProvoking == null)
            alienGunslingerReferences.CachedIdleOrIdleProvoking = IdleTypeSelection();

        return alienGunslingerReferences.CachedIdleOrIdleProvoking;
    }

    private Type IdleTypeSelection()
    {
        float sumChance = alienGunslingerReferences.IdleChance + alienGunslingerReferences.ProvokingChance;
        float chance = UnityEngine.Random.Range(0f, sumChance);

        if (chance < alienGunslingerReferences.IdleChance)
            return typeof(AGS_State_Idle);
        else
            return typeof(AGS_State_IdleProvoking);
    }

    private Type GetOrComputeAttackSelectionChance()
    {
        if (alienGunslingerReferences.CachedAttackType == null)
            alienGunslingerReferences.CachedAttackType = AttackSelectionChance();

        return alienGunslingerReferences.CachedAttackType;
    }

    private Type AttackSelectionChance()
    {
        float chancesSum = alienGunslingerReferences.SideToSideShootChance + alienGunslingerReferences.AroundHeadAttackChance + alienGunslingerReferences.DefendingChance + alienGunslingerReferences.DownUpShootChance;
        float chance = UnityEngine.Random.Range(0f, chancesSum);

        if (chance < alienGunslingerReferences.SideToSideShootChance)
            return typeof(AGS_State_SideToSideShoot);
        else if (chance < alienGunslingerReferences.SideToSideShootChance + alienGunslingerReferences.AroundHeadAttackChance)
            return typeof(ASG_State_AroundHeadAttack);
        else if (chance < alienGunslingerReferences.SideToSideShootChance + alienGunslingerReferences.AroundHeadAttackChance + alienGunslingerReferences.DefendingChance)
            return typeof(AGS_State_DefendingState);
        else
            return typeof(AGS_State_DownUpShoot);
    }

    private void Update() => stateMachine.Tick();

    private void Start() => SetupAimContraint();

    public void SetupAimContraint()
    {
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(alienGunslingerReferences.Character.transform, 1f));
        alienGunslingerReferences.HipsAimConstraint.data.sourceObjects = sourceObjects;
        alienGunslingerReferences.SpineAimContraint.data.sourceObjects = sourceObjects;
        alienGunslingerReferences.RigBuilder.Build();
    }

    private IEnumerator RecoveringShield()
    {
        yield return new WaitForSeconds(alienGunslingerReferences.TimeToRecoverShield);
        shieldRecovered = true;
        ShieldRecovered();
    }

    public void ShieldRecovered()
    {
        shieldBroken = false;
    }

    public Type GetCurrentState() => stateMachine.CurrentState.GetType();

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnShieldHitAction(PlayerTriedToAttack);
        EventManager.Instance.UnsubscribeFromOnStartSecondPhaseAction(StartSecondPhase);
        EventManager.Instance.UnsubscribeFromOnShieldBrokenAction(ShieldBroken);
    }
}
