using UnityEngine;
using UnityEngine.AI;

public class AlienSwordmasterReferences : MonoBehaviour
{
    [SerializeField] public CharacterController Character;
    [SerializeField] public float ChaseRange;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody;
    public float LastAttackTime;
    public AttackType NextAttack;
    public float RotatingSpeed;

    [SerializeField] public float AttackCooldown = 2f;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public AttackType DecideNextAttack()
    {
        NextAttack = Random.value < 0.5f ? AttackType.OutwardSlash : AttackType.InwardSlash;

        return NextAttack;
    }
}
