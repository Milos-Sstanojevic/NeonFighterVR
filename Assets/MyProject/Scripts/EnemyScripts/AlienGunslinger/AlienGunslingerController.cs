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
        AGS_State_SideWalkAndShoot sideWalkAndShoot = new AGS_State_SideWalkAndShoot(alienGunslingerReferences);
        ASG_State_AroundHeadAttack aroundHeadAttack = new ASG_State_AroundHeadAttack(alienGunslingerReferences);
        AGS_State_SideToSideShoot sideToSideShoot = new AGS_State_SideToSideShoot(alienGunslingerReferences);

        AGS_State_Idle idle = new AGS_State_Idle(alienGunslingerReferences);
        AGS_State_IdleProvoking idleProvoking = new AGS_State_IdleProvoking(alienGunslingerReferences, this);
        AGS_State_BrokenShieldIdle brokenShieldIdle = new AGS_State_BrokenShieldIdle(alienGunslingerReferences);
        AGS_State_DashFromPlayer dashFromPlayer = new AGS_State_DashFromPlayer(alienGunslingerReferences);
        AGS_State_RecoverShield recoverShield = new AGS_State_RecoverShield(alienGunslingerReferences);
        AGS_State_PunishPlayer punishPlayer = new AGS_State_PunishPlayer(alienGunslingerReferences);
        AGS_State_SideWalk sideWalk = new AGS_State_SideWalk(alienGunslingerReferences);

        AddTransition(drawGun, sideWalkAndShoot, () => drawGun.IsDone());

        // AddTransition(aroundHeadAttack, sideWalkAndShoot, () => aroundHeadAttack.IsDone() && !ShouldGoToIdleProvoking() && secondPhase);
        // AddTransition(aroundHeadAttack, idle, () => aroundHeadAttack.IsDone() && !ShouldGoToIdleProvoking() && ShouldSideWalkOrIdle() == typeof(AGS_State_Idle));
        // AddTransition(aroundHeadAttack, idleProvoking, () => aroundHeadAttack.IsDone() && ShouldGoToIdleProvoking()); //da li da dozvolim da ga provocira dok mu je aktivno pucanje oko glave?
        // AddTransition(aroundHeadAttack, sideWalk, () => !secondPhase && ShouldSideWalkOrIdle() == typeof(AGS_State_SideWalk) && aroundHeadAttack.IsDone());

        // AddTransition(sideWalkAndShoot, aroundHeadAttack, () => !alienGunslingerReferences.ShootingController.IsShooting() && GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !ShouldGoToIdleProvoking());
        // AddTransition(sideToSideShoot, idle, () => alienGunslingerReferences.SideToSideShootController.DoneShooting() && !alienGunslingerReferences.SideToSideShootController.ShotPlayer() && !ShouldGoToIdleProvoking());
        // AddTransition(sideToSideShoot, idleProvoking, () => alienGunslingerReferences.SideToSideShootController.DoneShooting() && !alienGunslingerReferences.SideToSideShootController.ShotPlayer() && ShouldGoToIdleProvoking());
        // AddTransition(sideToSideShoot, punishPlayer, () => alienGunslingerReferences.SideToSideShootController.ShotPlayer());

        // AddTransition(sideWalkAndShoot, sideToSideShoot, () => !alienGunslingerReferences.ShootingController.IsShooting() && GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && !ShouldGoToIdleProvoking());
        // AddTransition(sideWalkAndShoot, idle, () => !alienGunslingerReferences.ShootingController.IsShooting() && !ShouldGoToIdleProvoking());
        // AddTransition(sideWalkAndShoot, idleProvoking, () => !alienGunslingerReferences.ShootingController.IsShooting() && ShouldGoToIdleProvoking());

        // AddTransition(idle, sideToSideShoot, () => GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && !ShouldGoToIdleProvoking() && CanDoMultipleSpecialAttacks());
        // AddTransition(idle, aroundHeadAttack, () => GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !ShouldGoToIdleProvoking() && !alienGunslingerReferences.AroundHeadAttackController.HasActiveHolesAroundHead());
        // AddTransition(idle, sideWalkAndShoot, () => GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideWalkAndShoot) && !ShouldGoToIdleProvoking());
        // AddTransition(idle, idleProvoking, () => ShouldGoToIdleProvoking());
        // AddTransition(idle, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());

        // AddTransition(idleProvoking, sideToSideShoot, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && CanDoMultipleSpecialAttacks() && idleProvoking.IsDone());
        // AddTransition(idleProvoking, aroundHeadAttack, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !alienGunslingerReferences.AroundHeadAttackController.HasActiveHolesAroundHead() && idleProvoking.IsDone());
        // AddTransition(idleProvoking, sideWalkAndShoot, () => !playerTriedToAttack && GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideWalkAndShoot) && idleProvoking.IsDone());
        // AddTransition(idleProvoking, punishPlayer, () => playerTriedToAttack); //da li da dozvolim da ga punishuje dok ima rupe oko glave??
        // AddTransition(idleProvoking, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());

        // AddTransition(punishPlayer, idle, () => alienGunslingerReferences.ShootingController.IsPunishOver());

        // AddTransition(sideWalk, sideToSideShoot, () => GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideToSideShoot) && !ShouldGoToIdleProvoking() && CanDoMultipleSpecialAttacks());
        // AddTransition(sideWalk, aroundHeadAttack, () => GetOrComputeAttackSelectionChance() == typeof(ASG_State_AroundHeadAttack) && !ShouldGoToIdleProvoking() && !alienGunslingerReferences.AroundHeadAttackController.HasActiveHolesAroundHead());
        // AddTransition(sideWalk, sideWalkAndShoot, () => GetOrComputeAttackSelectionChance() == typeof(AGS_State_SideWalkAndShoot) && !ShouldGoToIdleProvoking());
        // AddTransition(sideWalk, idleProvoking, () => ShouldGoToIdleProvoking()); //da li da dozvolim da ga provocira dok ima rupe oko glave??

        // AddTransition(sideWalk, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());
        // AddTransition(sideWalkAndShoot, dashFromPlayer, () => alienGunslingerReferences.DashingController.IsDashing());

        // AddTransition(dashFromPlayer, idle, () => alienGunslingerReferences.DashingController.GetDashDone() && !ShouldGoToIdleProvoking());
        // AddTransition(dashFromPlayer, idleProvoking, () => alienGunslingerReferences.DashingController.GetDashDone() && ShouldGoToIdleProvoking());

        // Any(brokenShieldIdle, () => shieldBroken);
        // AddTransition(brokenShieldIdle, recoverShield, () => shieldRecovered);
        // AddTransition(recoverShield, idle, () => recoverShield.IsDone());

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

        // return alienGunslingerReferences.CachedAttackType;
        return typeof(AGS_State_SideToSideShoot);
    }

    private Type AttackSelectionChance()
    {
        float chancesSum = alienGunslingerReferences.SideToSideShootChance + alienGunslingerReferences.AroundHeadAttackChance + alienGunslingerReferences.SideWalkAndShootChance;
        float chance = UnityEngine.Random.Range(0f, chancesSum);

        if (chance < alienGunslingerReferences.SideToSideShootChance)
            return typeof(AGS_State_SideToSideShoot);
        else if (chance < alienGunslingerReferences.SideToSideShootChance + alienGunslingerReferences.AroundHeadAttackChance)
            return typeof(ASG_State_AroundHeadAttack);
        else
            return typeof(AGS_State_SideWalkAndShoot);
    }

    private Type ShouldSideWalkOrIdle()
    {
        float sumChance = alienGunslingerReferences.SideWalkChance + alienGunslingerReferences.IdleChance;
        float chance = UnityEngine.Random.Range(0f, sumChance);

        if (chance < alienGunslingerReferences.IdleChance)
            return typeof(AGS_State_Idle);
        else
            return typeof(AGS_State_SideWalk);
    }

    private void Update() => stateMachine.Tick();

    private void Start()
    {
        SetupAimContraint();
    }

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
