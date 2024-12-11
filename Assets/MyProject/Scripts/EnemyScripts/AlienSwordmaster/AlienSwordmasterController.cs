using System;
using UnityEngine;

public class AlienSwordmasterController : MonoBehaviour
{
    private AlienSwordmasterReferences alienSwordmasterReferences;
    private StateMachine enemyStateMachine;
    private bool firstAttackHit;

    private void Awake()
    {
        alienSwordmasterReferences = GetComponent<AlienSwordmasterReferences>();
        enemyStateMachine = new StateMachine();

        //STATES
        ASM_State_DrawSword drawSword = new ASM_State_DrawSword(alienSwordmasterReferences);
        ASM_State_DashToPlayer dashToPlayer = new ASM_State_DashToPlayer(alienSwordmasterReferences);
        ASM_State_Chase chaseState = new ASM_State_Chase(alienSwordmasterReferences);
        ASM_State_FightIdle fightIdleState = new ASM_State_FightIdle(alienSwordmasterReferences);
        ASM_State_OutwardSlash outwardSlash = new ASM_State_OutwardSlash(alienSwordmasterReferences);
        ASM_State_InwardSlash inwardSlash = new ASM_State_InwardSlash(alienSwordmasterReferences);

        //TRANSITIONS

        AddTransition(drawSword, fightIdleState, () => drawSword.IsDone());
        AddTransition(fightIdleState, dashToPlayer, () => ShouldDashToPlayer());
        AddTransition(dashToPlayer, fightIdleState, () => dashToPlayer.IsDone());


        AddTransition(fightIdleState, outwardSlash, () => IsInRangeForSlash() && fightIdleState.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(outwardSlash, fightIdleState, () => outwardSlash.IsDone() && !outwardSlash.AttackHit());
        AddTransition(fightIdleState, inwardSlash, () => IsInRangeForSlash() && fightIdleState.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(inwardSlash, fightIdleState, () => inwardSlash.IsDone());

        AddTransition(outwardSlash, inwardSlash, () => DidFirstAttackHit(outwardSlash));

        enemyStateMachine.SetState(drawSword);

        void AddTransition(IState from, IState to, Func<bool> condition) => enemyStateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => enemyStateMachine.AddAnyTransition(to, condition);
    }

    private bool DidFirstAttackHit(ASM_State_OutwardSlash outwardSlash)
    {
        firstAttackHit = true;
        return outwardSlash.AttackHit();
    }
    private bool ShouldDashToPlayer() => Vector3.Distance(transform.position, alienSwordmasterReferences.Character.transform.position) > 8;
    private bool IsInRangeForSlash() => Vector3.Distance(transform.position, alienSwordmasterReferences.Character.transform.position) <= 2;

    private void Update()
    {
        enemyStateMachine.Tick();
        FacePlayer();
    }

    private void FacePlayer()
    {
        // if (alienSwordmasterReferences.IsAttacing && !firstAttackHit)
        //     return;

        Vector3 directionToPlayer = alienSwordmasterReferences.Character.transform.position - transform.position;
        directionToPlayer.y = 0;
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * alienSwordmasterReferences.RotatingSpeed);
        }
    }
}
