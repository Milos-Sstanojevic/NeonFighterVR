using UnityEngine;

public class AGS_State_DefendingState : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_DefendingState(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("StandShoot", true);
        Debug.Log("Defending State");
        references.ShootingController.SetupDefendingFromShooting();
        references.CachedAttackType = null;
    }

    public void OnExit()
    {
        references.Animator.SetBool("StandShoot", false);
    }

    public void Tick()
    {
    }
}