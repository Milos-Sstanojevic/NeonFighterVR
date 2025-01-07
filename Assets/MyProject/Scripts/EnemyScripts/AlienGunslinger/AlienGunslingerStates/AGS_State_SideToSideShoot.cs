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
        references.CachedAttackType = null;
        references.SideToSideShootController.StartShooting();
    }

    public void OnExit()
    {
        references.Animator.SetBool("CrouchShoot", false);
    }

    public void Tick()
    {
    }
}