using UnityEngine;
using UnityEngine.AI;

public class AlienSwordmasterReferences : MonoBehaviour
{
    public Renderer EnemyRenderer;
    public GameObject EnemySword;
    public CharacterController Character;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Animator Animator { get; private set; }
    public float LastAttackTime;
    public AttackType NextAttack;
    public float RotatingSpeed;
    public float AttackCooldown = 2f;
    public float AttackRange = 2f;
    public float DashSpeed = 50;
    public float DelayAfterDashParticles = 1f;
    public ParticleSystem DashingParticleSystem;
    public MonoBehaviour Mono;


    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    public AttackType DecideNextAttack()
    {
        NextAttack = Random.value < 0.5f ? AttackType.OutwardSlash : AttackType.InwardSlash;

        return NextAttack;
    }
}
