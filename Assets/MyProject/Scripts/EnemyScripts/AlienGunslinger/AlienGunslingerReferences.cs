using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGunslingerReferences : MonoBehaviour
{
    public CharacterController Character;
    public AlienGSShootingController ShootingController;
    public AlienGSDashingController DashingController;
    public AlienGSAroundHeadAttackController AroundHeadAttackController;
    public AlienGSSideToSideShootController SideToSideShootController;
    public ShieldController ShieldController;
    public EnemyData EnemyData;
    public GunData GunData;
    public Animator Animator;
    public GameObject Shield;
    public MultiAimConstraint HipsAimConstraint;
    public MultiAimConstraint SpineAimContraint;
    public RigBuilder RigBuilder;
    public float SideWalkSpeed = 1.5f;
    public float TimeToRecoverShield = 2f;
    public float TimeForPlayerAttacking = 2f;
    public float ProvokingChance = 0.6f;
    public float SideWalkAndShootChance = 0.7f;
    public float SideToSideShootChance = 0.4f;
    public float AroundHeadAttackChance = 0.6f;
    public float IdleChance = 0.7f;
    public float SideWalkChance = 0.4f;
    public Type CachedAttackType;
    public Type CachedIdleOrIdleProvoking;


    private void Awake()
    {
        SideToSideShootController = GetComponent<AlienGSSideToSideShootController>();
        AroundHeadAttackController = GetComponent<AlienGSAroundHeadAttackController>();
        ShootingController = GetComponent<AlienGSShootingController>();
        DashingController = GetComponent<AlienGSDashingController>();
        Animator = GetComponent<Animator>();
        Character = Camera.main.GetComponentInParent<CharacterController>();
        RigBuilder = GetComponent<RigBuilder>();
    }
}