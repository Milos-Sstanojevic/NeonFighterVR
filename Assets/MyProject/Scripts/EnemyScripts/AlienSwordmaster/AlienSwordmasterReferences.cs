using System.Collections.Generic;
using UnityEngine;

public class AlienSwordmasterReferences : MonoBehaviour
{
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
