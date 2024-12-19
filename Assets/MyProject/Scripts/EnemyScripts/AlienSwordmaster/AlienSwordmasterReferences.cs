using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienSwordmasterReferences : MonoBehaviour
{
    public MultiAimConstraint MultiAimContraintHips;
    public MultiAimConstraint MultiAimContraintHead;
    public EnemyDamageController DamageController;
    public AlienSMBrokenStanceController BrokenStanceController;
    public RigBuilder RigBuilder;
    public CharacterController Character;
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody;
    public SlashAttack SlashAttack;
    public AlienSMDashAwayController DashFromController;
    public AlienSMDashToController DashToController;
    public AttackType NextAttack;
    public float LastAttackTime;
    public float RotatingSpeed;
    public float WalkSpeed = 3f;
    public float RunSpeed = 5f;
    public int SlashSpeed;
    public float SideWalkingTime = 1;
    public float AttackCooldown = 2f;
    public float AttackRange = 2f;
    public int NumberOfAttacksBeforeDashingAway = 3;
    public int NumberOfAttacksDone = 0;
    public bool IsAttacking;
    public bool AttackHit;

    private void Awake()
    {
        BrokenStanceController = GetComponent<AlienSMBrokenStanceController>();
        DamageController = GetComponent<EnemyDamageController>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Character = Camera.main.GetComponentInParent<CharacterController>();
    }
}

