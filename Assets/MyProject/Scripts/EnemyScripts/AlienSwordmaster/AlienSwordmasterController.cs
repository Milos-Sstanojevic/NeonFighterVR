using System;
using UnityEngine;
using UnityEngine.AI;

public class AlienSwordmasterController : MonoBehaviour
{
    [SerializeField] private bool runToPlayer;
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

        //TRANSITIONS
        AddTransition(drawSword, chaseState, () => drawSword.IsDone());

        AddTransition(chaseState, fightIdleState, () => ReachedPlayer());
        AddTransition(fightIdleState, chaseState, () => PlayerRunningAway());

        AddTransition(fightIdleState, outwardSlash, () => fightIdleState.ShouldDoOutwardSlash() && outwardSlash.CanAttack());
        AddTransition(outwardSlash, fightIdleState, () => outwardSlash.IsDone() && !outwardSlash.AttackHit());
        AddTransition(fightIdleState, inwardSlash, () => fightIdleState.ShouldDoInwardSlash() && inwardSlash.CanAttack());
        AddTransition(inwardSlash, fightIdleState, () => inwardSlash.IsDone());

        // // AddTransition(outwardSlash, inwardSlash, () => outwardSlash.AttackHit());

        enemyStateMachine.SetState(drawSword);

        void AddTransition(IState from, IState to, Func<bool> condition) => enemyStateMachine.AddTransition(from, to, condition);
        void Any(IState to, Func<bool> condition) => enemyStateMachine.AddAnyTransition(to, condition);
    }

    private bool ReachedPlayer() => alienSwordmasterReferences.NavMeshAgent.remainingDistance <= alienSwordmasterReferences.NavMeshAgent.stoppingDistance;

    private bool PlayerRunningAway() => Vector3.Distance(transform.position, alienSwordmasterReferences.Character.transform.position) >= 4f;

    private void Update() => enemyStateMachine.Tick();
}
