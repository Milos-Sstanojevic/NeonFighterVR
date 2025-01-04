using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGunslingerController : MonoBehaviour
{
    private bool playerTriedToAttack = false;
    private bool shotPlayer = false;//ovo nece postojati nego kad u shoot controlleru on pogodi igraca ovo se postavi na true
    private bool punishOver = false;
    private bool shieldBroken = false;
    public bool spawnAroundHead = false;
    public bool shouldSideToSideShoot = false; //mocice da udje u side shoot samo ako je prethodno vec ispalio ove oko glave
    private bool shieldRecovered;
    private StateMachine stateMachine;
    private AlienGunslingerReferences alienGunslingerReferences;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnShieldBrokenAction(ShieldBroken);
    }

    private void Awake()
    {
        stateMachine = new StateMachine();
        alienGunslingerReferences = GetComponent<AlienGunslingerReferences>();

        AGS_State_DrawGun drawGun = new AGS_State_DrawGun(alienGunslingerReferences);
        AGS_State_SideWalkAndShoot sideWalkAndShoot = new AGS_State_SideWalkAndShoot(alienGunslingerReferences);
        ASG_State_AroundHeadAttack aroundHeadAttack = new ASG_State_AroundHeadAttack(alienGunslingerReferences);
        AGS_State_SideToSideShoot sideToSideShoot = new AGS_State_SideToSideShoot(alienGunslingerReferences);

        AGS_State_Idle idle = new AGS_State_Idle(alienGunslingerReferences);
        AGS_State_IdleProvoking idleProvoking = new AGS_State_IdleProvoking(alienGunslingerReferences);
        AGS_State_BrokenShieldIdle brokenShieldIdle = new AGS_State_BrokenShieldIdle(alienGunslingerReferences);
        AGS_State_DashFromPlayer dashFromPlayer = new AGS_State_DashFromPlayer(alienGunslingerReferences);
        AGS_State_RecoverShield recoverShield = new AGS_State_RecoverShield(alienGunslingerReferences);
        AGS_State_PunishPlayer punishPlayer = new AGS_State_PunishPlayer(alienGunslingerReferences);

        AddTransition(drawGun, sideWalkAndShoot, () => drawGun.IsDone());
        AddTransition(sideWalkAndShoot, aroundHeadAttack, () => spawnAroundHead);
        AddTransition(aroundHeadAttack, sideWalkAndShoot, () => aroundHeadAttack.IsDone());

        AddTransition(sideWalkAndShoot, sideToSideShoot, () => shouldSideToSideShoot);
        AddTransition(sideToSideShoot, sideWalkAndShoot, () => sideToSideShoot.IsDone());

        AddTransition(sideWalkAndShoot, idle, () => !alienGunslingerReferences.ShootingController.ShouldShoot());
        AddTransition(sideWalkAndShoot, idleProvoking, () => ShouldGoToIdleProvoking());

        AddTransition(aroundHeadAttack, idle, () => aroundHeadAttack.IsDone());
        AddTransition(aroundHeadAttack, idleProvoking, () => ShouldGoToIdleProvoking());

        AddTransition(idle, sideToSideShoot, () => shouldSideToSideShoot);
        AddTransition(idle, aroundHeadAttack, () => spawnAroundHead);

        AddTransition(idleProvoking, sideToSideShoot, () => shouldSideToSideShoot);
        AddTransition(idleProvoking, aroundHeadAttack, () => spawnAroundHead);
        AddTransition(idleProvoking, sideWalkAndShoot, () => !playerTriedToAttack);

        AddTransition(idleProvoking, punishPlayer, () => playerTriedToAttack);
        AddTransition(punishPlayer, idle, () => punishOver);

        AddTransition(idle, sideWalkAndShoot, () => !shouldSideToSideShoot && !spawnAroundHead);
        AddTransition(sideToSideShoot, punishPlayer, () => shotPlayer);

        Any(brokenShieldIdle, () => shieldBroken);
        AddTransition(brokenShieldIdle, recoverShield, () => shieldRecovered);

        stateMachine.SetState(drawGun);
    }

    private void AddTransition(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition);

    private bool ShouldGoToIdleProvoking()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < alienGunslingerReferences.ProvokingChance)
            return true;
        else
            return false;
    }

    private void Update() => stateMachine.Tick();

    private void Start()
    {
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(alienGunslingerReferences.Character.transform, 1f));
        alienGunslingerReferences.HipsAimConstraint.data.sourceObjects = sourceObjects;
        alienGunslingerReferences.RigBuilder.Build();
    }

    public void ShieldBroken()
    {
        shieldBroken = true;
        shieldRecovered = false;
        StartCoroutine(RecoveringShield());
    }

    private IEnumerator RecoveringShield()
    {
        yield return new WaitForSeconds(alienGunslingerReferences.TimeToRecoverShield);
        shieldRecovered = true;
        ShieldRecovered();
    }

    public void ShieldRecovered()
    {
        shieldBroken = false;
    }

    public void PunishOver()
    {
        punishOver = true;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnShieldBrokenAction(ShieldBroken);
    }
}
