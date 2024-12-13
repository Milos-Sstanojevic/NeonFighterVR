using UnityEngine;
using UnityEngine.AI;

public class AlienSwordmasterReferences : MonoBehaviour
{
    public Renderer EnemyRenderer;
    public GameObject EnemySword;
    public CharacterController Character;
    public Animator Animator { get; private set; }
    public float LastAttackTime;
    public AttackType NextAttack;
    public float RotatingSpeed;
    public float WalkSpeed = 3f;
    public float RunSpeed = 5f;
    public float SideWalkingTime = 1;
    public float AttackCooldown = 2f;
    public float AttackRange = 2f;
    public float DashSpeed = 50;
    public float DashDistance = 8;
    public float DashAwayDistance = 9;
    public float DelayAfterDashParticles = 1f;
    public int NumberOfAttacksBeforeDashingAway = 3;
    public int NumberOfAttacksDone = 0;
    public ParticleSystem DashingParticleSystem;
    public MonoBehaviour Mono;
    public bool IsAttacing;
    public Rigidbody Rigidbody;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public AttackType DecideNextAttack()
    {
        NextAttack = Random.value < 0.5f ? AttackType.OutwardSlash : AttackType.InwardSlash;

        return NextAttack;
    }
}
