using System;
using UnityEngine;

public class AlienSwordmasterController : MonoBehaviour
{
    private AlienSwordmasterReferences alienSwordmasterReferences;
    private EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        alienSwordmasterReferences = GetComponent<AlienSwordmasterReferences>();
        enemyStateMachine = new EnemyStateMachine();

        //STATES
        ASM_State_DrawSword drawSword = new ASM_State_DrawSword(alienSwordmasterReferences);
        ASM_State_Chase chaseState = new ASM_State_Chase(alienSwordmasterReferences);
        ASM_State_FightIdle fightIdleState = new ASM_State_FightIdle(alienSwordmasterReferences);
        ASM_State_OutwardSlash outwardSlash = new ASM_State_OutwardSlash(alienSwordmasterReferences);
        ASM_State_InwardSlash inwardSlash = new ASM_State_InwardSlash(alienSwordmasterReferences);
        ASM_State_DashForwardUpAttack dashForwardUpAttack = new ASM_State_DashForwardUpAttack(alienSwordmasterReferences);
        
        //TRANSITIONS

        AddTransition(drawSword, fightIdleState, () => drawSword.IsDone());
        AddTransition(fightIdleState, dashForwardUpAttack, () => ShouldDashToPlayer());
        AddTransition(dashForwardUpAttack, fightIdleState, () => dashForwardUpAttack.IsDone());


        AddTransition(fightIdleState, outwardSlash, () => IsInRangeForSlash() && fightIdleState.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(outwardSlash, fightIdleState, () => outwardSlash.IsDone() && !outwardSlash.AttackHit());
        AddTransition(fightIdleState, inwardSlash, () => IsInRangeForSlash() && fightIdleState.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(inwardSlash, fightIdleState, () => inwardSlash.IsDone());

        AddTransition(outwardSlash, inwardSlash, () => outwardSlash.AttackHit());

        enemyStateMachine.SetState(drawSword);

        void AddTransition(IState from, IState to, Func<bool> condition) => enemyStateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => enemyStateMachine.AddAnyTransition(to, condition);
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
        Vector3 directionToPlayer = alienSwordmasterReferences.Character.transform.position - transform.position;
        directionToPlayer.y = 0;
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * alienSwordmasterReferences.RotatingSpeed);
        }
    }
}