public class AGS_State_SideWalkAndShoot : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_SideWalkAndShoot(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("StandShoot", true);
        references.ShootingController.SetupShooting();
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