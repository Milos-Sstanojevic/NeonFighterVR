using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGunslingerReferences : MonoBehaviour
{
    public CharacterController Character;
    public AlienGSShootingController ShootingController;
    public AlienGSDashingController DashingController;
    public ShieldController ShieldController;
    public EnemyData EnemyData;
    public Animator Animator;
    public GameObject Shield;
    public MultiAimConstraint HipsAimConstraint;
    public MultiAimConstraint LeftHandAimConstraint;
    public MultiAimConstraint RightHandAimConstraint;
    public RigBuilder RigBuilder;
    public float SideWalkSpeed = 1.5f;
    public float TimeToRecoverShield = 2f;
    public float ProvokingChance = 0.6f;


    private void Awake()
    {
        ShootingController = GetComponent<AlienGSShootingController>();
        DashingController = GetComponent<AlienGSDashingController>();
        Animator = GetComponent<Animator>();
        Character = Camera.main.GetComponentInParent<CharacterController>();
        RigBuilder = GetComponent<RigBuilder>();
    }
}