using UnityEngine;

public class AGS_State_SideToSideShoot : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_SideToSideShoot(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("CrouchShoot", true);
        Debug.Log("Side To Side Shoot");
        references.CachedAttackType = null;
        references.SideToSideShootController.StartShooting();
    }

    public void OnExit()
    {
        references.SideToSideShootController.ResetShootingBools();
        references.Animator.SetBool("CrouchShoot", false);
    }

    public void Tick()
    {
    }
}