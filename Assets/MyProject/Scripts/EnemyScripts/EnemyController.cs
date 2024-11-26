using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CharacterController character;
    private EnemyReferences enemyReferences;
    private EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        enemyStateMachine = new EnemyStateMachine();

        //STATES
        EnemyState_RunToPlayer runToPlayer = new EnemyState_RunToPlayer(enemyReferences, character);
        EnemyState_Delay wait = new EnemyState_Delay(2f);
        EnemyState_StartCombat roar = new EnemyState_StartCombat(enemyReferences);
        EnemyState_SwipeAttack swipeAttack = new EnemyState_SwipeAttack(enemyReferences);

        EnemyState_PunchAttack punchAttack = new EnemyState_PunchAttack(enemyReferences);
        wait = new EnemyState_Delay(3f);

        //TRANSITIONS
        AddTransition(runToPlayer, wait, () => runToPlayer.HasArrivedAtDestination());
        AddTransition(wait, roar, () => wait.IsDone());
        AddTransition(roar, swipeAttack, () => roar.IsDone());
        AddTransition(swipeAttack, punchAttack, () => swipeAttack.IsDone());

        AddTransition(punchAttack, wait, () => punchAttack.IsDone());
        AddTransition(wait, punchAttack, () => wait.IsDone());

        enemyStateMachine.SetState(runToPlayer);

        void AddTransition(IState from, IState to, Func<bool> condition) => enemyStateMachine.AddTransition(from: from, to: to, condition);
        void Any(IState to, Func<bool> condition) => enemyStateMachine.AddAnyTransition(to, condition);
    }

    private void Update() => enemyStateMachine.Tick();
}