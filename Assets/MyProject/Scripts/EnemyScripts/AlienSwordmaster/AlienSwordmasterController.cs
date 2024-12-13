using System;
using UnityEngine;

public class AlienSwordmasterController : MonoBehaviour
{
    private AlienSwordmasterReferences alienSwordmasterReferences;
    private StateMachine enemyStateMachine;
    public bool HasDash = false; //this will be bool that will be set true for second fase of the boss
    public bool SpecialAttack = false;

    private void Awake()
    {
        alienSwordmasterReferences = GetComponent<AlienSwordmasterReferences>();
        enemyStateMachine = new StateMachine();

        //STATES
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
        //TRANSITIONS
        AddTransition(drawSword, dashToPlayer, () => drawSword.IsDone() && HasDash);
        AddTransition(drawSword, runTowardsPlayer, () => drawSword.IsDone() && !HasDash);

        //SECOND PHASE WITH DASH
        AddTransition(fightIdleState, dashToPlayer, () => alienSwordmasterReferences.DashToController.ShouldDashToPlayer() && HasDash);  //this will alien do at some other condition not like this
        AddTransition(dashToPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack() && HasDash);
        AddTransition(dashToPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack() && HasDash);
        AddTransition(outwardSlash, dashAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && outwardSlash.IsDone() && !outwardSlash.AttackHit() && HasDash);
        AddTransition(inwardSlash, dashAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && inwardSlash.IsDone() && HasDash);
        AddTransition(dashAwayFromPlayer, sideWalk, () => alienSwordmasterReferences.DashFromController.IsDone());
        AddTransition(sideWalk, dashToPlayer, () => sideWalk.StopWalking() && HasDash && !SpecialAttack);   //bool for spacial attack should be somewhat random WIP
        AddTransition(sideWalk, specialSlashAttack, () => sideWalk.StopWalking() && SpecialAttack && HasDash);
        AddTransition(specialSlashAttack, dashToPlayer, () => specialSlashAttack.IsDone());


        //FIRST PHASE NO DASH
        AddTransition(outwardSlash, jumpAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && outwardSlash.IsDone() && !outwardSlash.AttackHit() && !HasDash);
        AddTransition(inwardSlash, jumpAwayFromPlayer, () => ShouldMakeDistanceFromPlayer() && inwardSlash.IsDone() && !HasDash);
        AddTransition(jumpAwayFromPlayer, sideWalk, () => jumpAwayFromPlayer.IsDone());
        AddTransition(sideWalk, walkTowardsPlayer, () => sideWalk.StopWalking() && !HasDash);
        AddTransition(runTowardsPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack() && !HasDash);
        AddTransition(runTowardsPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack() && !HasDash);

        //GENERAL TRANSITIONS
        AddTransition(fightIdleState, dashAwayFromPlayer, () => !IsInRangeForSlash() && ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(fightIdleState, walkTowardsPlayer, () => !IsInRangeForSlash() && !ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(outwardSlash, walkTowardsPlayer, () => !IsInRangeForSlash() && outwardSlash.IsDone() && !ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(inwardSlash, walkTowardsPlayer, () => !IsInRangeForSlash() && inwardSlash.IsDone() && !ShouldDashFromPlayerOrWalkToPlayer());
        AddTransition(walkTowardsPlayer, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(walkTowardsPlayer, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(fightIdleState, inwardSlash, () => IsInRangeForSlash() && inwardSlash.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(fightIdleState, outwardSlash, () => IsInRangeForSlash() && outwardSlash.ShouldDoOutwardSlash() && outwardSlash.CanAttack());

        AddTransition(outwardSlash, fightIdleState, () => outwardSlash.IsDone() && !outwardSlash.AttackHit());
        AddTransition(inwardSlash, fightIdleState, () => inwardSlash.IsDone());

        AddTransition(outwardSlash, inwardSlash, () => outwardSlash.AttackHit()); //combo of two moves

        enemyStateMachine.SetState(drawSword);

        void AddTransition(IState from, IState to, Func<bool> condition) => enemyStateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => enemyStateMachine.AddAnyTransition(to, condition);
    }

    public bool IsInRangeForSlash() => Vector3.Distance(transform.position, alienSwordmasterReferences.Character.transform.position) <= alienSwordmasterReferences.AttackRange;

    private void Update()
    {
        enemyStateMachine.Tick();
        FacePlayer();
        if (transform.position.y != 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void FacePlayer()
    {
        if (alienSwordmasterReferences.IsAttacking)
            return;

        Vector3 directionToPlayer = alienSwordmasterReferences.Character.transform.position - transform.position;
        directionToPlayer.y = 0;
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * alienSwordmasterReferences.RotatingSpeed);
        }
    }

    public bool ShouldMakeDistanceFromPlayer()
    {
        if (alienSwordmasterReferences.NumberOfAttacksDone <= alienSwordmasterReferences.NumberOfAttacksBeforeDashingAway)
            return false;

        return true;
    }

    private bool ShouldDashFromPlayerOrWalkToPlayer()
    {
        if (!HasDash)
            return false;

        return UnityEngine.Random.value < alienSwordmasterReferences.DashFromController.GetDashChance();
    }
}
