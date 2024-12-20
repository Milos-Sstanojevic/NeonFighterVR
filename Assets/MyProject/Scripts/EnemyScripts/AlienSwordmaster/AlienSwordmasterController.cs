using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienSwordmasterController : MonoBehaviour
{
    private AlienSwordmasterReferences alienSwordmasterReferences;
    private StateMachine firstPhaseStateMachine;
    private StateMachine secondPhaseStateMachine;
    public bool HasDash = false; //this will be bool that will be set true for second fase of the boss
    public bool SpecialAttack = false;

    private void Start()
    {
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(alienSwordmasterReferences.Character.transform, 1));
        alienSwordmasterReferences.MultiAimContraintHips.data.sourceObjects = sourceObjects;
        alienSwordmasterReferences.MultiAimContraintHead.data.sourceObjects = sourceObjects;
        alienSwordmasterReferences.RigBuilder.Build();
    }

    private void Awake()
    {
        alienSwordmasterReferences = GetComponent<AlienSwordmasterReferences>();

        firstPhaseStateMachine = new StateMachine();
        secondPhaseStateMachine = new StateMachine();

        ASM_State_DrawSword drawSword = new ASM_State_DrawSword(alienSwordmasterReferences);
        ASM_State_DashToPlayer dashToPlayer = new ASM_State_DashToPlayer(alienSwordmasterReferences);
        ASM_State_DashAwayFromPlayer dashAwayFromPlayer = new ASM_State_DashAwayFromPlayer(alienSwordmasterReferences);
        ASM_State_FightIdle fightIdleState = new ASM_State_FightIdle(alienSwordmasterReferences);
        ASM_State_OutwardSlash outwardSlash = new ASM_State_OutwardSlash(alienSwordmasterReferences);
        ASM_State_InwardSlash inwardSlash = new ASM_State_InwardSlash(alienSwordmasterReferences);
        ASM_State_SideWalk sideWalk = new ASM_State_SideWalk(alienSwordmasterReferences);
        ASM_State_WalkTowardsPlayer walkTowardsPlayer = new ASM_State_WalkTowardsPlayer(alienSwordmasterReferences);
        ASM_State_JumpAwayFromPlayer jumpAwayFromPlayer = new ASM_State_JumpAwayFromPlayer(alienSwordmasterReferences);
        ASM_State_RunTowardsPlayer runTowardsPlayer = new ASM_State_RunTowardsPlayer(alienSwordmasterReferences);
        ASM_State_SpecialSlashAttack specialSlashAttack = new ASM_State_SpecialSlashAttack(alienSwordmasterReferences);
        ASM_State_TakeDamage takeDamage = new ASM_State_TakeDamage(alienSwordmasterReferences);
        ASM_State_BreakStance breakStance = new ASM_State_BreakStance(alienSwordmasterReferences);


        AddFirstPhaseTransitions(firstPhaseStateMachine, fightIdleState, drawSword, runTowardsPlayer, outwardSlash, inwardSlash, jumpAwayFromPlayer, sideWalk, walkTowardsPlayer, takeDamage, breakStance);

        AddSecondPhaseTransitions(secondPhaseStateMachine, fightIdleState, walkTowardsPlayer, drawSword, dashToPlayer, dashAwayFromPlayer, outwardSlash, inwardSlash, sideWalk, specialSlashAttack, breakStance, takeDamage);

        firstPhaseStateMachine.SetState(drawSword);
        secondPhaseStateMachine.SetState(drawSword);
    }

    private void AddFirstPhaseTransitions(StateMachine firstPhaseStateMachine, ASM_State_FightIdle fightIdleState, ASM_State_DrawSword drawSword, ASM_State_RunTowardsPlayer runTowardsPlayer, ASM_State_OutwardSlash outwardSlash, ASM_State_InwardSlash inwardSlash,
                                        ASM_State_JumpAwayFromPlayer jumpAwayFromPlayer, ASM_State_SideWalk sideWalk, ASM_State_WalkTowardsPlayer walkTowardsPlayer, ASM_State_TakeDamage takeDamage, ASM_State_BreakStance breakStance)
    {
        AddTransition(firstPhaseStateMachine, runTowardsPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(firstPhaseStateMachine, drawSword, runTowardsPlayer, () => drawSword.IsDone());
        AddTransition(firstPhaseStateMachine, runTowardsPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(firstPhaseStateMachine, outwardSlash, jumpAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && outwardSlash.IsDone() && !alienSwordmasterReferences.AttackHit);
        AddTransition(firstPhaseStateMachine, inwardSlash, jumpAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && inwardSlash.IsDone());
        AddTransition(firstPhaseStateMachine, jumpAwayFromPlayer, sideWalk, () => jumpAwayFromPlayer.IsDone());
        AddTransition(firstPhaseStateMachine, sideWalk, walkTowardsPlayer, () => sideWalk.StopWalking());
        AddTransition(firstPhaseStateMachine, fightIdleState, walkTowardsPlayer, () => !IsInRangeForSlash());
        AddTransition(firstPhaseStateMachine, outwardSlash, walkTowardsPlayer, () => !IsInRangeForSlash() && outwardSlash.IsDone());
        AddTransition(firstPhaseStateMachine, inwardSlash, walkTowardsPlayer, () => !IsInRangeForSlash() && inwardSlash.IsDone());
        AddTransition(firstPhaseStateMachine, walkTowardsPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(firstPhaseStateMachine, walkTowardsPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(firstPhaseStateMachine, fightIdleState, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(firstPhaseStateMachine, fightIdleState, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(firstPhaseStateMachine, outwardSlash, fightIdleState, () => outwardSlash.IsDone() && !alienSwordmasterReferences.AttackHit);
        AddTransition(firstPhaseStateMachine, inwardSlash, fightIdleState, () => IsInRangeForSlash() && inwardSlash.IsDone());
        AddTransition(firstPhaseStateMachine, outwardSlash, inwardSlash, () => alienSwordmasterReferences.AttackHit);

        Any(firstPhaseStateMachine, breakStance, () => alienSwordmasterReferences.DamageController.ReceivedBigDamage());
        AddTransition(firstPhaseStateMachine, breakStance, takeDamage, () => alienSwordmasterReferences.DamageController.ReceivedDamage() && breakStance.IsDone());
        AddTransition(firstPhaseStateMachine, breakStance, jumpAwayFromPlayer, () => alienSwordmasterReferences.BrokenStanceController.RecoverFromBrokenStance() && breakStance.IsDone());
    }

    private void AddSecondPhaseTransitions(StateMachine secondPhaseStateMachine, ASM_State_FightIdle fightIdleState, ASM_State_WalkTowardsPlayer walkTowardsPlayer,
                                    ASM_State_DrawSword drawSword, ASM_State_DashToPlayer dashToPlayer, ASM_State_DashAwayFromPlayer dashAwayFromPlayer, ASM_State_OutwardSlash outwardSlash,
                                     ASM_State_InwardSlash inwardSlash, ASM_State_SideWalk sideWalk, ASM_State_SpecialSlashAttack specialSlashAttack, ASM_State_BreakStance breakStance, ASM_State_TakeDamage takeDamage)
    {
        AddTransition(secondPhaseStateMachine, drawSword, dashToPlayer, () => drawSword.IsDone());
        AddTransition(secondPhaseStateMachine, dashToPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(secondPhaseStateMachine, dashToPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());

        AddTransition(secondPhaseStateMachine, dashToPlayer, walkTowardsPlayer, () => !IsInRangeForSlash() && !ShouldDashFromPlayerOrWalkToPlayer() && alienSwordmasterReferences.DashToController.IsDone());
        AddTransition(secondPhaseStateMachine, dashToPlayer, dashAwayFromPlayer, () => !IsInRangeForSlash() && ShouldDashFromPlayerOrWalkToPlayer() && alienSwordmasterReferences.DashToController.IsDone());

        AddTransition(secondPhaseStateMachine, dashAwayFromPlayer, sideWalk, () => alienSwordmasterReferences.DashFromController.IsDone());
        AddTransition(secondPhaseStateMachine, sideWalk, dashToPlayer, () => sideWalk.StopWalking() && !SpecialAttack);   //bool for spacial attack should be somewhat random WIP
        AddTransition(secondPhaseStateMachine, sideWalk, specialSlashAttack, () => sideWalk.StopWalking() && SpecialAttack);
        AddTransition(secondPhaseStateMachine, specialSlashAttack, dashToPlayer, () => specialSlashAttack.IsDone());

        AddTransition(secondPhaseStateMachine, outwardSlash, dashAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && outwardSlash.IsDone() && !alienSwordmasterReferences.AttackHit);
        AddTransition(secondPhaseStateMachine, inwardSlash, dashAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && inwardSlash.IsDone());
        AddTransition(secondPhaseStateMachine, fightIdleState, dashAwayFromPlayer, () => !IsInRangeForSlash() && ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(secondPhaseStateMachine, fightIdleState, walkTowardsPlayer, () => !IsInRangeForSlash() && !ShouldDashFromPlayerOrWalkToPlayer());

        AddTransition(secondPhaseStateMachine, outwardSlash, walkTowardsPlayer, () => !IsInRangeForSlash() && outwardSlash.IsDone() && !ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(secondPhaseStateMachine, inwardSlash, walkTowardsPlayer, () => !IsInRangeForSlash() && inwardSlash.IsDone() && !ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(secondPhaseStateMachine, walkTowardsPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(secondPhaseStateMachine, walkTowardsPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(secondPhaseStateMachine, fightIdleState, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(secondPhaseStateMachine, fightIdleState, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());

        AddTransition(secondPhaseStateMachine, outwardSlash, fightIdleState, () => outwardSlash.IsDone() && !alienSwordmasterReferences.AttackHit);
        AddTransition(secondPhaseStateMachine, inwardSlash, fightIdleState, () => IsInRangeForSlash() && inwardSlash.IsDone());

        AddTransition(secondPhaseStateMachine, outwardSlash, inwardSlash, () => alienSwordmasterReferences.AttackHit);

        Any(secondPhaseStateMachine, breakStance, () => alienSwordmasterReferences.DamageController.ReceivedBigDamage());
        AddTransition(secondPhaseStateMachine, breakStance, takeDamage, () => alienSwordmasterReferences.DamageController.ReceivedDamage() && breakStance.IsDone());
        AddTransition(secondPhaseStateMachine, breakStance, dashAwayFromPlayer, () => alienSwordmasterReferences.BrokenStanceController.RecoverFromBrokenStance() && breakStance.IsDone());
    }

    private void AddTransition(StateMachine stateMachine, IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(StateMachine stateMachine, IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);

    public bool IsInRangeForSlash() => (transform.position - alienSwordmasterReferences.Character.transform.position).sqrMagnitude <= alienSwordmasterReferences.AttackRange * alienSwordmasterReferences.AttackRange;

    private void Update()
    {
        if (HasDash)
            secondPhaseStateMachine.Tick();
        else
            firstPhaseStateMachine.Tick();

        if (transform.position.y != 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public bool ShouldMakeDistanceFromPlayer()
    {
        if (alienSwordmasterReferences.NumberOfAttacksDone < alienSwordmasterReferences.NumberOfAttacksBeforeDashingAway)
            return false;

        return true;
    }

    private bool ShouldDashFromPlayerOrWalkToPlayer()
    {
        return UnityEngine.Random.value < alienSwordmasterReferences.DashFromController.GetDashChance();
    }
}
